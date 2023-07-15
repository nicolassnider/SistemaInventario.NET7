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
    public class BodegaRepository : Repository<Bodega>, IBodegaRepository
    {
        private readonly ApplicationDbContext _db;
        public BodegaRepository(ApplicationDbContext db) : base(db) /*envía a clase padre*/
        {
            _db = db;
        }

        public void Update(Bodega bodega)
        {
            var bodegaBD = _db.Bodegas.FirstOrDefault(b => b.Id == bodega.Id);
            if (bodegaBD != null)
            {
                bodegaBD.Nombre=bodega.Nombre;
                bodegaBD.Descripcion =bodega.Descripcion;
                bodegaBD.Estado=bodega.Estado;
                _db.SaveChanges();
            }

        }
    }
}
