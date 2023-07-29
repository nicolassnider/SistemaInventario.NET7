using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Models.ViewModels;
using SistemaInventario.Utilidades;
using System.Security.Claims;

namespace SistemaInventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =DS.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Upsert()
        {
            CompaniaVM companiaVM = new CompaniaVM()
            {
                Compania =new Models.Compania(),
                BodegaLista = _unitOfWork.Compania.GetAllDropdownList("Bodega"),
            };
            companiaVM.Compania = await _unitOfWork.Compania.GetFirstOrDefault();
            return View(companiaVM);

        }
        #region API
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(CompaniaVM companiaVM)
        {
            if (ModelState.IsValid)
            {
                TempData[DS.Exitosa] = "Compania grabada correctamente";
                var claimIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

                if (companiaVM.Compania.Id == null)
                {
                    //crear compañía
                    companiaVM.Compania.CreadoPorId = claim.Value;
                    companiaVM.Compania.ActualizadoPorId = claim.Value;
                    companiaVM.Compania.FechaCreacion = DateTime.Now;
                    companiaVM.Compania.FechaActualizacion = DateTime.Now;
                    await _unitOfWork.Compania.Add(companiaVM.Compania);
                }
                else
                {
                    //update compañia
                    companiaVM.Compania.ActualizadoPorId = claim.Value;
                    companiaVM.Compania.FechaActualizacion = DateTime.Now;
                }
                await _unitOfWork.Save();
                return RedirectToAction("Index", "Home");
            }
            TempData[DS.Error] = "Error al grabar";
            return View(companiaVM);
        }
            
        
        #endregion
    }
}
