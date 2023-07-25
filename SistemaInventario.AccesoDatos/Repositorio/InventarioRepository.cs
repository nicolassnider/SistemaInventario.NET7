using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class InventarioRepository:Repository<Inventario>,IInventarioRepository
    {
        private readonly ApplicationDbContext _db;
        public InventarioRepository(ApplicationDbContext db):base(db)
        {
            _db = db;            
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
