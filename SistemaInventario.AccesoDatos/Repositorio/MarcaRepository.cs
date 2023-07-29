using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Models;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class MarcaRepository : Repository<Marca>, IMarcaRepository
    {
        private readonly ApplicationDbContext _db;
        public MarcaRepository(ApplicationDbContext db) : base(db) /*envía a clase padre*/
        {
            _db = db;
        }

        public void Update(Marca marca)
        {
            var marcaBD = _db.Marcas.FirstOrDefault(b => b.Id == marca.Id);
            if (marcaBD != null)
            {
                marcaBD.Nombre = marca.Nombre;
                marcaBD.Descripcion = marca.Descripcion;
                marcaBD.Estado = marca.Estado;
                _db.SaveChanges();
            }

        }
    }
}
