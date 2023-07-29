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
    public class CarroComprasRepository : Repository<CarroCompras>, ICarroComprasRepository
    {
        private readonly ApplicationDbContext _db;

        public CarroComprasRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(CarroCompras carroCompras)
        {
            _db.Update(carroCompras);
        }
    }
}
