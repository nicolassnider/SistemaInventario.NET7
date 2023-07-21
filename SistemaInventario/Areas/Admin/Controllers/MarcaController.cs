using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Models;
using SistemaInventario.Utilidades;

namespace SistemaInventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MarcaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public MarcaController(IUnitOfWork unitOfWork)
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
            Marca marca = new Marca();
            if (id == null)
            {
                //crea nueva Categoria
                marca.Estado = true;
                return View(marca);
            }
            //actualiza marca
            marca = await _unitOfWork.Marca.Get(id.GetValueOrDefault());
            if (marca == null)
            {
                return NotFound();
            }
            return View(marca);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Marca marca)
        {
            if (ModelState.IsValid)
            {
                if (marca.Id == 0)
                {
                    await _unitOfWork.Marca.Add(marca);
                    TempData[DS.Exitosa] = "Marca creada exitosamente";
                }
                else
                {
                    _unitOfWork.Marca.Update(marca);
                    TempData[DS.Exitosa] = "Marca modificada exitosamente";
                }
                await _unitOfWork.Save();
                return RedirectToAction(nameof(Index)); //invoca a Index
            }
            TempData[DS.Error] = "Error al manejar esta marca";
            return View(marca);

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var marcas = await _unitOfWork.Marca.GetAll();
            return Json(new { data = marcas });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var marcaDb = await _unitOfWork.Marca.Get(id);
            if (marcaDb == null)
            {
                return Json(new
                { success = false, message = "Error al borrar marca" });
            }
            _unitOfWork.Marca.Remove(marcaDb);
            await _unitOfWork.Save();
            return Json(new { succes = true, message = "Marca eliminada correctamente" });
        }
        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unitOfWork.Marca.GetAll();
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
