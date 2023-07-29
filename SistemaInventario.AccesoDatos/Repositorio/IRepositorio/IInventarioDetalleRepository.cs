using SistemaInventario.Models;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface IInventarioDetalleRepository : IRepository<InventarioDetalle>
    {
        void Update(InventarioDetalle inventarioDetalle);
    }
}
