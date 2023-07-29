using SistemaInventario.Models;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface IMarcaRepository : IRepository<Marca>
    {
        void Update(Marca marca);

    }
}
