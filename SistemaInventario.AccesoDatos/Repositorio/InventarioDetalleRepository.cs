using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Models;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class InventarioDetalleRepository : Repository<InventarioDetalle>, IInventarioDetalleRepository
    {
        private readonly ApplicationDbContext _db;
        public InventarioDetalleRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(InventarioDetalle inventarioDetalle)
        {
            var inventarioDetalleBD = _db.InventarioDetalles.FirstOrDefault(i => i.Id == inventarioDetalle.Id);
            if (inventarioDetalleBD != null)
            {
                inventarioDetalleBD.StockAnterior = inventarioDetalle.StockAnterior;
                inventarioDetalleBD.Cantidad = inventarioDetalle.Cantidad;
                _db.SaveChanges();
            }

        }
    }
}
