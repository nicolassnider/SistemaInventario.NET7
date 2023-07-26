using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Utilidades;

namespace SistemaInventario.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    [Authorize(Roles= DS.Role_Inventario+","+DS.Role_Admin)]
    public class InventarioController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public InventarioController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
