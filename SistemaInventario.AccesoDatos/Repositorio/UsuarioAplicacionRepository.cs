using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Models;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class UsuarioAplicacionRepository: Repository<UsuarioAplicacion>, IUsuarioAplicacionRepositorio
    {
        private readonly ApplicationDbContext _db;
        public UsuarioAplicacionRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
    }
}
