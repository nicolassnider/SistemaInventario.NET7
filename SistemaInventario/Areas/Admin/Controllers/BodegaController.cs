using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Models;

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
                }
                else
                {
                    _unitOfWork.Bodega.Update(bodega);
                }
                await _unitOfWork.Save();
                return RedirectToAction(nameof(Index)); //invoca a Index
            }
            return View(bodega);
            
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bodegas = await _unitOfWork.Bodega.GetAll();
            return Json(new {data = bodegas});
        }        
        #endregion
    }
}
