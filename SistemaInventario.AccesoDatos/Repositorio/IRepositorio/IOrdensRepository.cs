using SistemaInventario.Models;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface IOrdenRepository:IRepository<Orden>
    {
        void Update(Orden orden);
    }
}
