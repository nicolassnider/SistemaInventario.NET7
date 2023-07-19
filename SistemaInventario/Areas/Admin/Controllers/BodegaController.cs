using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Models;
using SistemaInventario.Utilidades;

namespace SistemaInventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BodegaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BodegaController(IUnitOfWork unitOfWork)
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
            Bodega bodega = new Bodega();
            if (id==null)
            {
                //crea nueva Bodega
                bodega.Estado = true;
                return View(bodega);
            }
            //actualiza bodega
            bodega = await _unitOfWork.Bodega.Get(id.GetValueOrDefault());
            if (bodega ==null)
            {

                return NotFound();
            }
            return View(bodega);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Upsert(Bodega bodega)
        {
            if (ModelState.IsValid)
            {
                if (bodega.Id==0)
                {
                    await _unitOfWork.Bodega.Add(bodega);
                    TempData[DS.Exitosa]="Bodega creada exitosamente";
                }
                else
                {
                    _unitOfWork.Bodega.Update(bodega);
                    TempData[DS.Exitosa] = "Bodega modificada exitosamente";
                }
                await _unitOfWork.Save();
                return RedirectToAction(nameof(Index)); //invoca a Index
            }
            TempData[DS.Error] = "Error al manejar esta bodega";
            return View(bodega);
            
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bodegas = await _unitOfWork.Bodega.GetAll();
            return Json(new {data = bodegas});
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var bodegaDb = await _unitOfWork.Bodega.Get(id);
            if (bodegaDb == null) {
                return Json(new 
                { success = false,message="Error al borrar bodega" });
            }
            _unitOfWork.Bodega.Remove(bodegaDb);
            await _unitOfWork.Save();
            return Json(new { succes = true, message = "Bodega eliminada correctamente" });
        }
        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id=0)
        {
            bool valor = false;
            var lista = await _unitOfWork.Bodega.GetAll();
            if (id == 0)
            {
                valor = lista.Any(b=>b.Nombre.Trim().ToLower() == nombre.Trim().ToLower());
            }
            else
            {
                valor = lista.Any(b => b.Nombre.Trim().ToLower() == nombre.Trim().ToLower() && b.Id!=id);
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
