using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaInventario.Models;
using SistemaInventario.Models.Especificaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface IBodegaProductoRepository : IRepository<BodegaProducto>
    {
        void Update(BodegaProducto bodegaProducto);
        IEnumerable<SelectList> SelectAllDropdownList(string obj);
    }
}
