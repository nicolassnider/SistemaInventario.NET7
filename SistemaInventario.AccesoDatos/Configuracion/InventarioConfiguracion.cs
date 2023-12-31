﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaInventario.Models;

namespace SistemaInventario.AccesoDatos.Configuracion
{
    public class InventarioConfiguracion : IEntityTypeConfiguration<Inventario>
    {
        public void Configure(EntityTypeBuilder<Inventario> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x=>x.BodegaId).IsRequired();
            builder.Property(x=>x.UsuarioAplicacionId).IsRequired();
            builder.Property(x=>x.FechaFinal).IsRequired();
            builder.Property(x=>x.FechaInicial).IsRequired();

            /*Relaciones*/

            builder.HasOne(x=>x.UsuarioAplicacion).WithMany()
                   .HasForeignKey(x=>x.UsuarioAplicacionId)
                   .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Bodega).WithMany()
                   .HasForeignKey(x => x.BodegaId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
