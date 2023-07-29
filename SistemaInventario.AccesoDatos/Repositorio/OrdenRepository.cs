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
    public class OrdenRepository : Repository<Orden>, IOrdenRepository
    {
        private readonly ApplicationDbContext _db;

        public OrdenRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(Orden orden)
        {
            _db.Update(orden);
        }
    }
}
