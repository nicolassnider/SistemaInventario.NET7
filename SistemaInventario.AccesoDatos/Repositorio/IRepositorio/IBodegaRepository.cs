using SistemaInventario.Models;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface IBodegaRepository:IRepository<Bodega>
    {
        void Update(Bodega bodega);

    }
}
