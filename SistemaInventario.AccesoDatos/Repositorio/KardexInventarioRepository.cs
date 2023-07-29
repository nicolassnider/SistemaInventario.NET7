using Microsoft.EntityFrameworkCore;
using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Models;
using SistemaInventario.Utilidades;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class KardexInventarioRepository : Repository<KardexInventario>, IKardexInventarioRepository
    {
        private readonly ApplicationDbContext _db;
        public KardexInventarioRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task RegistrarKardex(int bodegaProductoId, string tipo, string detalle, int stockAnterior, int cantidad, string usuarioId)
        { 
            var bodegaProducto = await _db.BodegasProductos.Include(b => b.Producto).FirstOrDefaultAsync(b => b.Id == bodegaProductoId);

            if (tipo == DS.Tipo_Entrada || tipo == DS.Tipo_Salida)
            {
                KardexInventario kardex = new KardexInventario
                {
                    BodegaProductoId = bodegaProductoId,
                    Tipo = tipo,
                    Detalle = detalle,
                    StockAnterior = stockAnterior,
                    Cantidad = cantidad,
                    Costo = bodegaProducto.Producto.Costo,
                    Stock = tipo == DS.Tipo_Entrada ? stockAnterior + cantidad : stockAnterior - cantidad,
                    UsuarioAplicacionId = usuarioId,
                    FechaRegistro = DateTime.Now
                };

                kardex.Total = kardex.Stock * kardex.Costo;

                await _db.kardexInventarios.AddAsync(kardex);
                await _db.SaveChangesAsync();
            }
        }
    }
}
