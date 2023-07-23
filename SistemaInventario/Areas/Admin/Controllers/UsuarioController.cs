using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Utilidades;
using System.Data;

namespace SistemaInventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin)]
    public class UsuarioController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;

        public UsuarioController(IUnitOfWork unitOfWork,ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }


        public IActionResult Index()
        {
            return View();
        }
        #region API
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuariosLista = await _unitOfWork.UsuarioAplicacion.GetAll();
            var userRole = await _db.UserRoles.ToListAsync();
            var roles = await _db.Roles.ToListAsync();

            foreach (var user in usuariosLista) {
                var roleId = userRole.FirstOrDefault(u=>u.UserId==user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
            }
            return Json(new { data = usuariosLista });
            
        }

        [HttpPost]
        public async Task<ActionResult> BloquearDesbloquear([FromBody] string id)
        {
            var usuario = await _unitOfWork.UsuarioAplicacion.GetFirstOrDefault(u => u.Id == id);
            if (usuario == null) return Json(new { success = false, message = "Error de usuario" });
            if(usuario.LockoutEnd!=null && usuario.LockoutEnd > DateTime.Now)
            {
                //usuario bloqueado
                usuario.LockoutEnd = DateTime.Now;
            }
            else
            {
                usuario.LockoutEnd= DateTime.Now.AddYears(100);
            }
            await _unitOfWork.Save();
            return Json(new { success = true, message = "operación exitosa" });
        }

        #endregion
    }
}
