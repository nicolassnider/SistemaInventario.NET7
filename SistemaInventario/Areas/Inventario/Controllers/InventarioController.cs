using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Identity.Client;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Models;
using SistemaInventario.Models.ViewModels;
using SistemaInventario.Utilidades;
using System.Security.Claims;

namespace SistemaInventario.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    [Authorize(Roles= DS.Role_Inventario+","+DS.Role_Admin)]
    public class InventarioController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]//la utilizamos en todo el modelo y permanentemente cambiará su valor
        public InventarioVM inventarioVM { get; set; }

        public InventarioController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NuevoInventario()
        {
            inventarioVM = new InventarioVM()
            {
                Inventario= new Models.Inventario(), //genera conflictos por repetir el nombre
                BodegaList = _unitOfWork.Inventario.GetAllDropdownList("Bodega"),
                //no es necesario completar todos las propiedades
            };
            inventarioVM.Inventario.Estado = false;
            //obtener id de usuario
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            inventarioVM.Inventario.UsuarioAplicacionId = claim.Value;
            inventarioVM.Inventario.FechaInicial = DateTime.Now;
            inventarioVM.Inventario.FechaFinal = DateTime.Now;
            return View(inventarioVM);
        }

        public async Task<IActionResult> DetalleInventario(int id)
        {
            inventarioVM = new InventarioVM();
            inventarioVM.Inventario = await _unitOfWork.Inventario
                .GetFirstOrDefault(i=>i.Id == id,includeProperties:"Bodega"); 
            inventarioVM.InventarioDetalles = await _unitOfWork.InventarioDetalle
                .GetAll(d=>d.InventarioId==id,includeProperties:"Producto,Producto.Marca");//la marca está asociada al producto
            return View(inventarioVM);
        }

        public async Task<IActionResult>Mas(int id) //recibe id del detalle
        {
            inventarioVM =new InventarioVM();
            var detalle = await _unitOfWork.InventarioDetalle.Get(id);
            inventarioVM.Inventario = await _unitOfWork.Inventario.Get(detalle.InventarioId);
            detalle.Cantidad += 1;
            await _unitOfWork.Save();
            return RedirectToAction("DetalleInventario", new { id = inventarioVM.Inventario.Id });

        }
        public async Task<IActionResult> Menos(int id) //recibe id del detalle
        {
            inventarioVM = new InventarioVM();
            var detalle = await _unitOfWork.InventarioDetalle.Get(id);
            inventarioVM.Inventario = await _unitOfWork.Inventario.Get(detalle.InventarioId);
            if (detalle.Cantidad==1)
            {
                _unitOfWork.InventarioDetalle.Remove(detalle);
            }
            else
            {
                detalle.Cantidad -= 1;
            }
            await _unitOfWork.Save();
            return RedirectToAction("DetalleInventario", new { id = inventarioVM.Inventario.Id });

        }

        public async Task<IActionResult>GenerarStock(int id)
        {
            var inventario = await _unitOfWork.Inventario.Get(id);
            var detalleLista = await _unitOfWork.InventarioDetalle.GetAll(d => d.InventarioId == id);

            foreach (var item in detalleLista)
            {
                var bodegaProducto = new BodegaProducto();
                bodegaProducto = await _unitOfWork.BodegaProducto
                    .GetFirstOrDefault(b => b.ProductoId == item.ProductoId && b.BodegaID == inventario.BodegaId, isTracking: false);
                if (inventario!=null)//el registro de stock existe , actualizar cantidad
                {
                    bodegaProducto.Cantidad += item.Cantidad;
                    await _unitOfWork.Save();
                }
                else //Registro no existe, crear
                {
                    bodegaProducto=new BodegaProducto();
                    bodegaProducto.BodegaID = inventario.BodegaId;
                    bodegaProducto.ProductoId = item.ProductoId;
                    bodegaProducto.Cantidad = item.Cantidad;
                    await _unitOfWork.BodegaProducto.Add(bodegaProducto);
                    await _unitOfWork.Save();
                }
            }
            //actualizar cabecera
            inventario.Estado = true;
            inventario.FechaFinal = DateTime.Now;
            await _unitOfWork.Save();
            return RedirectToAction("Index");
        }


        #region API
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetalleInventario(int InventarioId, int productoId, int cantidadId)
        {
            inventarioVM = new InventarioVM();
            inventarioVM.Inventario = await _unitOfWork.Inventario.GetFirstOrDefault(i => i.Id == InventarioId);
            var bodegaProducto = await _unitOfWork.BodegaProducto
                .GetFirstOrDefault(b => b.BodegaID == inventarioVM.Inventario.BodegaId && b.ProductoId == productoId);
            var detalle = await _unitOfWork.InventarioDetalle
                .GetFirstOrDefault(d=>d.InventarioId == InventarioId && d.ProductoId==productoId);

            if (detalle==null)
            {
                inventarioVM.InventarioDetalle = new Models.InventarioDetalle();
                inventarioVM.InventarioDetalle.ProductoId = productoId;
                inventarioVM.InventarioDetalle.InventarioId = InventarioId;
                if (bodegaProducto!=null)
                {
                    inventarioVM.InventarioDetalle.StockAnterior = bodegaProducto.Cantidad;
                }
                else
                {
                    inventarioVM.InventarioDetalle.StockAnterior = 0;
                }
                inventarioVM.InventarioDetalle.Cantidad = cantidadId;
                await _unitOfWork.InventarioDetalle.Add(inventarioVM.InventarioDetalle);
                await _unitOfWork.Save();
            }
            return RedirectToAction("DetalleInventario", new {id=InventarioId});

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NuevoInventario(InventarioVM inventarioVM)
        {
            if (ModelState.IsValid)
            {
                inventarioVM.Inventario.FechaInicial=DateTime.Now;
                inventarioVM.Inventario.FechaFinal=DateTime.Now;
                await _unitOfWork.Inventario.Add(inventarioVM.Inventario);
                await _unitOfWork.Save();
                return RedirectToAction("DetalleInventario", new {id=inventarioVM.Inventario.Id});
            }
            inventarioVM.BodegaList = _unitOfWork.Inventario.GetAllDropdownList("Bodega");
            return View(inventarioVM);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var todos = await _unitOfWork.BodegaProducto.GetAll(includeProperties: "Bodega,Producto");
            return Json(new {data=todos});

        }
        [HttpGet]
        public async Task<IActionResult>BuscarProducto(string term)
        {
            if (!string.IsNullOrEmpty(term))
            {
                var listaProductos = await _unitOfWork.Producto.GetAll(p => p.Estado == true);
                var data = listaProductos.Where(x => 
                    x.NumeroSerie.Contains(term, StringComparison.OrdinalIgnoreCase)|| 
                    x.Descripcion.Contains(term, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                return Ok(data);
            }
            return Ok();
        }
        #endregion
    }
}
