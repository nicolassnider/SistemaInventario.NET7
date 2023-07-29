using SistemaInventario.Models;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        void Update(Categoria categoria);

    }
}
