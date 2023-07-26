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
        public InventarioVM InventarioVM { get; set; }

        public InventarioController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult NuevoInventario()
        {
            InventarioVM = new InventarioVM()
            {
                Inventario= new Models.Inventario(), //genera conflictos por repetir el nombre
                BodegaList = _unitOfWork.Inventario.GetAllDropdownList("Bodega"),
                //no es necesario completar todos las propiedades
            };
            InventarioVM.Inventario.Estado = false;
            //obtener id de usuario
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            InventarioVM.Inventario.UsuarioAplicacionId = claim.Value;
            InventarioVM.Inventario.FechaInicial = DateTime.Now;
            InventarioVM.Inventario.FechaFinal = DateTime.Now;
            return View(InventarioVM);
        }
        public IActionResult Index()
        {
            return View();
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var todos = await _unitOfWork.BodegaProducto.GetAll(includeProperties: "Bodega,Producto");
            return Json(new {data=todos});

        }
        #endregion
    }
}
