using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Models;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class InventarioRepository:Repository<Inventario>,IInventarioRepository
    {
        private readonly ApplicationDbContext _db;
        public InventarioRepository(ApplicationDbContext db):base(db)
        {
            _db = db;            
        }

        public IEnumerable<SelectListItem> GetAllDropdownList(string obj)
        {
            if (obj == "Bodega")
            {
                return _db.Bodegas.Where(b=>b.Estado==true).Select(b=>new SelectListItem {Text=b.Nombre,Value=b.Id.ToString()});
            }
            return null;
        }

        public void Update(Inventario inventario)
        {
            var inventarioBD = _db.Inventarios.FirstOrDefault(i=>i.Id== inventario.Id);
            if (inventarioBD != null)
            {
                inventarioBD.BodegaId=inventario.BodegaId;
                inventarioBD.FechaFinal=inventario.FechaFinal;
                inventarioBD.Estado=inventario.Estado;

                _db.SaveChanges();
            }

        }
    }
}
