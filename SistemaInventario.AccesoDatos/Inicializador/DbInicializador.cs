using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.Models;
using SistemaInventario.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Inicializador
{
    public class DbInicializador : IDbInicializador
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInicializador(ApplicationDbContext db, 
            UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Inicializar()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            //datos iniciales
            if (_db.Roles.Any(r => r.Name == DS.Role_Admin)) return;
            _roleManager.CreateAsync(new IdentityRole(DS.Role_Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(DS.Role_Cliente)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(DS.Role_Inventario)).GetAwaiter().GetResult();
            //crear usuario admin
            _userManager.CreateAsync(new UsuarioAplicacion{
                UserName="nicolas.snider@gmail.com",
                Email="nicolas.snider@gmail.com",
                EmailConfirmed=true,
                Nombres="Nicolas",
                Apellido="Snider"
            }, "Admin!123!").GetAwaiter().GetResult();
            //asignar rol
            UsuarioAplicacion user = _db.UsuariosAplicacion
                .Where(u => u.UserName == "nicolas.snider@gmail.com").FirstOrDefault();
            _userManager.AddToRoleAsync(user, DS.Role_Admin).GetAwaiter().GetResult();
        }
    }
}
