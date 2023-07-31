﻿using Microsoft.AspNetCore.Authorization;
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
