using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Models;
using SistemaInventario.Models.ViewModels;
using SistemaInventario.Utilidades;
using System.Security.Claims;

namespace SistemaInventario.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class CarroComprasController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public CarroComprasVM carroComprasVM { get; set; }

        public CarroComprasController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

        private Claim obtenerUsuarioId()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return claim;
        }
    }
}
