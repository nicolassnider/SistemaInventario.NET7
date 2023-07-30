using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Models;
using SistemaInventario.Models.ViewModels;
using SistemaInventario.Utilidades;
using Stripe.Checkout;
using System.Security.Claims;

namespace SistemaInventario.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class CarroComprasController : Controller
    {
        
        private readonly IUnitOfWork _unitOfWork;
        private string _webUrl;
        [BindProperty]
        public CarroComprasVM carroComprasVM { get; set; }

        public CarroComprasController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _webUrl = configuration.GetValue<string>("DomainUrls:WEB_URL");
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var claim = obtenerUsuarioId();
            carroComprasVM = new CarroComprasVM();
            carroComprasVM.Orden = new Models.Orden();
            carroComprasVM.CarroCompraLista = await _unitOfWork.CarroCompras
                .GetAll(u => u.UsuarioAplicacionId == claim.Value,
                        includeProperties:"Producto");
            carroComprasVM.Orden.TotalOrden = 0;
            carroComprasVM.Orden.UsuarioAplicacionId = claim.Value;
            foreach (var lista in carroComprasVM.CarroCompraLista)
            {
                lista.Precio = lista.Producto.Precio; //precio actual del producto
                carroComprasVM.Orden.TotalOrden += (lista.Precio * lista.Cantidad);
            }
            return View(carroComprasVM);
        }

        public async Task<IActionResult> mas(int carroId)
        {
            var carroCompras = await _unitOfWork.CarroCompras.GetFirstOrDefault(c => c.Id == carroId);
            carroCompras.Cantidad = +1;
            await _unitOfWork.Save();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> menos(int carroId)
        {
            var carroCompras = await _unitOfWork.CarroCompras.GetFirstOrDefault(c => c.Id == carroId);
            if (carroCompras.Cantidad == 1)
            {
                //remover registro del carro de compra y actualizar sesion
                var carroLista = await _unitOfWork.CarroCompras
                    .GetAll(c => c.UsuarioAplicacionId == carroCompras.UsuarioAplicacionId);
                var numeroProductos = carroLista.Count();
                _unitOfWork.CarroCompras.Remove(carroCompras);
                HttpContext.Session.SetInt32(DS.ssCarroCompras, numeroProductos - 1);
            }
            else
            {
                carroCompras.Cantidad -= 1;
                await _unitOfWork.Save();
            }
            
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> remover(int carroId)
        {
            //remueve registro del carro de compras y actualiza la sesión
            var carroCompras = await _unitOfWork.CarroCompras.GetFirstOrDefault(c => c.Id == carroId);
            var carroLista = await _unitOfWork.CarroCompras
                    .GetAll(c => c.UsuarioAplicacionId == carroCompras.UsuarioAplicacionId);
            var numeroProductos = carroLista.Count();
            _unitOfWork.CarroCompras.Remove(carroCompras);
            await _unitOfWork.Save();
            HttpContext.Session.SetInt32(DS.ssCarroCompras, numeroProductos - 1);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Proceder()
        {
            var claim = obtenerUsuarioId();
            carroComprasVM = new CarroComprasVM()
            {
                Orden = new Models.Orden(),
                CarroCompraLista = await _unitOfWork.CarroCompras
                .GetAll(c => c.UsuarioAplicacionId == claim.Value,
                        includeProperties: "Producto"),
                Compania = await _unitOfWork.Compania.GetFirstOrDefault()
            };
            carroComprasVM.Orden.TotalOrden = 0;
            carroComprasVM.Orden.UsuarioAplicacion = await _unitOfWork.UsuarioAplicacion
                .GetFirstOrDefault(u => u.Id == claim.Value);

            foreach (var lista in carroComprasVM.CarroCompraLista)
            {
                lista.Precio = lista.Producto.Precio;
                carroComprasVM.Orden.TotalOrden += (lista.Precio * lista.Cantidad);
            }
            carroComprasVM.Orden.NombresCliente =
                $"{carroComprasVM.Orden.UsuarioAplicacion.Nombres} {carroComprasVM.Orden.UsuarioAplicacion.Apellido}";
            carroComprasVM.Orden.Telefono = carroComprasVM.Orden.UsuarioAplicacion.PhoneNumber;
            carroComprasVM.Orden.Direccion = carroComprasVM.Orden.UsuarioAplicacion.Direccion;
            carroComprasVM.Orden.Pais = carroComprasVM.Orden.UsuarioAplicacion.Pais;
            carroComprasVM.Orden.Ciudad = carroComprasVM.Orden.UsuarioAplicacion.Ciudad;

            //control stock
            foreach(var lista in carroComprasVM.CarroCompraLista)
            {
                //capturar stock de c/ producto
                var producto = await _unitOfWork.BodegaProducto
                    .GetFirstOrDefault(b => b.ProductoId == lista.ProductoId &&
                                       b.BodegaID == carroComprasVM.Compania.BodegaVentaId);
                if (lista.Cantidad>producto.Cantidad)
                {
                    TempData[DS.Error] = $"Cantidad de producto {lista.Producto.Descripcion} excede el stock actual ({producto.Cantidad})";
                    return RedirectToAction("Index");
                }
            }
            return View(carroComprasVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Proceder(CarroComprasVM carroComprasVM)
        {
            var claim = obtenerUsuarioId();
            carroComprasVM.CarroCompraLista = await _unitOfWork.CarroCompras
                .GetAll(c => c.UsuarioAplicacionId == claim.Value,
                           includeProperties: "Producto");
            carroComprasVM.Compania = await _unitOfWork.Compania.GetFirstOrDefault();
            carroComprasVM.Orden.TotalOrden = 0;
            carroComprasVM.Orden.UsuarioAplicacionId= claim.Value;
            carroComprasVM.Orden.FechaOrden = DateTime.Now;
            foreach (var lista in carroComprasVM.CarroCompraLista)
            {
                lista.Precio = lista.Producto.Precio;
                carroComprasVM.Orden.TotalOrden += (lista.Precio + lista.Cantidad);
            }
            //control stock
            foreach (var lista in carroComprasVM.CarroCompraLista)
            {
                //capturar stock de c/ producto
                var producto = await _unitOfWork.BodegaProducto
                    .GetFirstOrDefault(b => b.ProductoId == lista.ProductoId &&
                                       b.BodegaID == carroComprasVM.Compania.BodegaVentaId);
                if (lista.Cantidad > producto.Cantidad)
                {
                    TempData[DS.Error] = $"Cantidad de producto {lista.Producto.Descripcion} excede el stock actual ({producto.Cantidad})";
                    return RedirectToAction("Index");
                }
            }
            carroComprasVM.Orden.EstadoOrden = DS.EstadoPendiente;
            carroComprasVM.Orden.EstadoPago = DS.PagoEstadoPendiente;
            await _unitOfWork.Orden.Add(carroComprasVM.Orden);
            await _unitOfWork.Save();
            //guardar detalle orden

            foreach (var lista in carroComprasVM.CarroCompraLista)
            {
                OrdenDetalle ordenDetalle = new OrdenDetalle()
                {
                    ProductoId = lista.ProductoId,
                    OrdenId = carroComprasVM.Orden.Id,
                    Precio = lista.Precio,
                    Cantidad = lista.Cantidad
                };
                await _unitOfWork.OrdenDetalle.Add(ordenDetalle);
                await _unitOfWork.Save();
            }
            //stripe
            var user = await _unitOfWork.UsuarioAplicacion.GetFirstOrDefault(u => u.Id == claim.Value);
            var options = new SessionCreateOptions
            {
                SuccessUrl=_webUrl+$"inventario/carroCompras/OrdenConfirmacion?id={carroComprasVM.Orden.Id}",
                CancelUrl= _webUrl+"inventario/carroCompras/index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode="payment",
                CustomerEmail=user.Email
            };
            foreach (var lista in carroComprasVM.CarroCompraLista)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData= new SessionLineItemPriceDataOptions()
                    {
                        UnitAmount=(long)(lista.Precio*100), //siempre multiplicar por 100
                        Currency = "usd",
                        ProductData=new SessionLineItemPriceDataProductDataOptions
                        {
                            Name=lista.Producto.Descripcion
                        },
                    },
                    Quantity=lista.Cantidad,
                };
                options.LineItems.Add(sessionLineItem);
            }
            var service = new SessionService();
            Session session = service.Create(options);
            _unitOfWork.Orden
                .UpdatePagoStripeId(carroComprasVM.Orden.Id, session.Id, session.PaymentIntentId);
            await _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);//redireccion a stripe
            return new StatusCodeResult(StatusCodes.Status303SeeOther);
           
        }
        public async Task<IActionResult>OrdenConfirmacion(int id)
        {
            var orden = await _unitOfWork.Orden
                .GetFirstOrDefault(o=>o.Id==id, 
                                      includeProperties:"UsuarioAplicacion");
            var service = new SessionService();
            Session session = service.Get(orden.SessionId);
            var carroCompra = await _unitOfWork.CarroCompras
                .GetAll(u => u.UsuarioAplicacionId == orden.UsuarioAplicacionId);
            if (session.PaymentStatus.ToLower()=="paid")
            {
                _unitOfWork.Orden.UpdatePagoStripeId(id, session.Id, session.PaymentIntentId);
                _unitOfWork.Orden.UpdateEstado(id, DS.EstadoAprobado, DS.PagoEstadoAprobado);
                await _unitOfWork.Save();
                //disminuir stock de la bodega de venta
                var compania = await _unitOfWork.Compania.GetFirstOrDefault();
                foreach(var lista in carroCompra)
                {
                    var bodegaProducto = new BodegaProducto();
                    bodegaProducto = await _unitOfWork.BodegaProducto
                        .GetFirstOrDefault(b=>b.ProductoId==lista.ProductoId &&
                                              b.BodegaID==compania.BodegaVentaId);

                    await _unitOfWork.KardexInventario
                        .RegistrarKardex(bodegaProducto.Id, 
                                         DS.Tipo_Salida, 
                                         "Venta - Orden#" + id,
                                         bodegaProducto.Cantidad, 
                                         lista.Cantidad, 
                                         orden.UsuarioAplicacionId);
                    bodegaProducto.Cantidad -= lista.Cantidad;
                    await _unitOfWork.Save();
                }
            }
            //borrar carro y la sesion del caarro
            
            List<CarroCompras> carroComprasLista = carroCompra.ToList();
            _unitOfWork.CarroCompras.RemoveRange(carroComprasLista);
            await _unitOfWork.Save();
            HttpContext.Session.SetInt32(DS.ssCarroCompras, 0);
            return View();
        }

        private Claim obtenerUsuarioId()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return claim;
        }
    }
}
