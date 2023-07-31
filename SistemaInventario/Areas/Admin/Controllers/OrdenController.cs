using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Models;
using SistemaInventario.Models.ViewModels;
using SistemaInventario.Utilidades;
using System.Security.Claims;

namespace SistemaInventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrdenController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        private OrdenDetalleVm ordenDetalleVm { get; set; }
        public OrdenController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        
        public IActionResult Index()
        {
            return View();
        }

        private Claim obtenerUsuarioId()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return claim;
        }
        public async Task<IActionResult>Detalle(int id)
        {
            ordenDetalleVm = new OrdenDetalleVm()
            {
                Orden = await _unitOfWork.Orden
                .GetFirstOrDefault(o=>o.Id== id,
                                      includeProperties:"UsuarioAplicacion"),
                OrdenDetalleLista = await _unitOfWork.OrdenDetalle
                .GetAll(d=>d.OrdenId==id,
                           includeProperties:"Producto")
            };
            return View(ordenDetalleVm);
        }
        [Authorize(Roles = DS.Role_Admin)]
        public async Task<IActionResult>Procesar(int id)
        {
            var orden = await _unitOfWork.Orden
                .GetFirstOrDefault(o => o.Id == id);
            orden.EstadoOrden = DS.EstadoEnProceso;
            await _unitOfWork.Save();
            TempData[DS.Exitosa] = "Orden cambiada a estado en Proceso";
            return RedirectToAction("Detalle", new { id = id });

        }
        [HttpPost]
        [Authorize(Roles = DS.Role_Admin)]
        public async Task<IActionResult> EnviarOrden(OrdenDetalleVm ordenDetalleVm)
        {
            var orden = await _unitOfWork.Orden
                .GetFirstOrDefault(o => o.Id == ordenDetalleVm.Orden.Id);
            orden.EstadoOrden = DS.EstadoEnviado;
            orden.Carrier = ordenDetalleVm.Orden.Carrier;
            orden.NumeroEnvio = ordenDetalleVm.Orden.NumeroEnvio;
            orden.FechaEnvio = DateTime.Now;
            await _unitOfWork.Save();
            TempData[DS.Exitosa] = "Orden Enviada";
            return RedirectToAction("Detalle", new { id = ordenDetalleVm.Orden.Id });

        }
        #region
        [HttpGet]
        public async Task<IActionResult> ObtenerOrdenLista(string estado)
        {
            var claim = obtenerUsuarioId();
            IEnumerable<Orden> todos;
            if (User.IsInRole(DS.Role_Admin))//validar rol del us
            {
                todos = await _unitOfWork.Orden
                    .GetAll(includeProperties: "UsuarioAplicacion");
            }else
            {
                todos = await _unitOfWork.Orden
                    .GetAll(o=>o.UsuarioAplicacionId==claim.Value, includeProperties: "UsuarioAplicacion");
            }

            switch (estado)
            {
                case "aprobado":
                    todos = todos.Where(o => o.EstadoOrden == DS.EstadoAprobado);
                    break;
                case "completado":
                    todos = todos.Where(o => o.EstadoOrden == DS.EstadoEnviado);
                    break;
                default:
                    break;

            }
            return Json(new { data = todos });
        }
        #endregion
    }
}
