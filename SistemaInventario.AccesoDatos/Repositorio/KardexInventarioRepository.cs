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
    public class KardexInventarioRepository : Repository<KardexInventario>, IKardexInventarioRepository
    {
        private readonly ApplicationDbContext _db;
        public KardexInventarioRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }        
    }
}
