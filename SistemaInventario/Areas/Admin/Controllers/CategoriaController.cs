using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Models;
using SistemaInventario.Utilidades;

namespace SistemaInventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin + ","+DS.Role_Inventario)]
    public class CategoriaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoriaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region API
        public async Task<IActionResult> Upsert(int? id)
        {
            Categoria categoria = new Categoria();
            if (id == null)
            {
                //crea nueva Categoria
                categoria.Estado = true;
                return View(categoria);
            }
            //actualiza categoria
            categoria = await _unitOfWork.Categoria.Get(id.GetValueOrDefault());
            if (categoria == null)
            {

                return NotFound();
            }
            return View(categoria);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                if (categoria.Id == 0)
                {
                    await _unitOfWork.Categoria.Add(categoria);
                    TempData[DS.Exitosa] = "Categoria creada exitosamente";
                }
                else
                {
                    _unitOfWork.Categoria.Update(categoria);
                    TempData[DS.Exitosa] = "Categoria modificada exitosamente";
                }
                await _unitOfWork.Save();
                return RedirectToAction(nameof(Index)); //invoca a Index
            }
            TempData[DS.Error] = "Error al manejar esta categoria";
            return View(categoria);

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categorias = await _unitOfWork.Categoria.GetAll();
            return Json(new { data = categorias });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var categoriaDb = await _unitOfWork.Categoria.Get(id);
            if (categoriaDb == null)
            {
                return Json(new
                { success = false, message = "Error al borrar categoria" });
            }
            _unitOfWork.Categoria.Remove(categoriaDb);
            await _unitOfWork.Save();
            return Json(new { succes = true, message = "Categoria eliminada correctamente" });
        }
        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unitOfWork.Categoria.GetAll();
            if (id == 0)
            {
                valor = lista.Any(b => b.Nombre.Trim().ToLower() == nombre.Trim().ToLower());
            }
            else
            {
                valor = lista.Any(b => b.Nombre.Trim().ToLower() == nombre.Trim().ToLower() && b.Id != id);
            }
            if (valor)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });
        }

        #endregion
    }
}
