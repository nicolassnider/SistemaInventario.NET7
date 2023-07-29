using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaInventario.Models;

namespace SistemaInventario.AccesoDatos.Configuracion
{
    internal class BodegaProductoConfiguracion : IEntityTypeConfiguration<BodegaProducto>
    {
        public void Configure(EntityTypeBuilder<BodegaProducto> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x=>x.ProductoId).IsRequired();
            builder.Property(x=>x.BodegaID).IsRequired();
            builder.Property(x=>x.Cantidad).IsRequired();
            //relaciones
            builder
                .HasOne(x => x.Bodega).WithMany()
                .HasForeignKey(x => x.BodegaID)
                .OnDelete(DeleteBehavior.NoAction);
            builder
                .HasOne(x => x.Producto).WithMany()
                .HasForeignKey(x => x.ProductoId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
