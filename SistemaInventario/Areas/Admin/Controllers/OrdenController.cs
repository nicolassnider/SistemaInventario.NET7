using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;

namespace SistemaInventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrdenController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrdenController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public IActionResult Index()
        {
            return View();
        }
        #region
        [HttpGet]
        public async Task<IActionResult> ObtenerOrdenLista()
        {
            var todos = await _unitOfWork.Orden.GetAll(includeProperties:"UsuarioAplicacion");
            return Json(new { data = todos });
        }
        #endregion
    }
}
