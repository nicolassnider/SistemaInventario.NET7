using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
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


        #region API
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
