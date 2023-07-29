using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Models;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class CompaniaRepository : Repository<Compania>, ICompaniaRepository
    {
        private readonly ApplicationDbContext _db;

        public CompaniaRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(Compania compania)
        {
            var companiaBD = _db.Companias.FirstOrDefault(c => c.Id == compania.Id);
            if (companiaBD != null)
            {
                companiaBD.Nombre = compania.Nombre;
                companiaBD.Descripcion = compania.Descripcion;
                compania.Pais = compania.Pais;
                companiaBD.Ciudad = compania.Ciudad;
                companiaBD.Direccion = compania.Direccion;
                companiaBD.Telefono= compania.Telefono;
                companiaBD.BodegaVentaId = compania.BodegaVentaId;
                companiaBD.ActualizadoPorId = compania.ActualizadoPorId;
                companiaBD.FechaActualizacion = compania.FechaActualizacion;
                _db.SaveChanges();
            }
        }
    }
}
