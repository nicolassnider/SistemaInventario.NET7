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
    public class OrdenDetalleRepository : Repository<OrdenDetalle>, IOrdenDetalleRepository
    {
        private readonly ApplicationDbContext _db;

        public OrdenDetalleRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(OrdenDetalle ordenDetalle)
        {
            _db.Update(ordenDetalle);
        }
    }
}
