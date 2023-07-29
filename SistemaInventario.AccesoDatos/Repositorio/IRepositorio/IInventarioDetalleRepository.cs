using SistemaInventario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface IInventarioDetalleRepository : IRepository<InventarioDetalle>
    {
        void Update(InventarioDetalle inventarioDetalle);
    }
}
