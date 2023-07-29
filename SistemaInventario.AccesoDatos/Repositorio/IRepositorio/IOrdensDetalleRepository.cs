using SistemaInventario.Models;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface IOrdenDetalleRepository:IRepository<OrdenDetalle>
    {
        void Update(OrdenDetalle ordenDetalle);
    }
}
