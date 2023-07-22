using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaInventario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface IProductoRepository : IRepository<Producto>
    {
        void Update(Producto producto);
        IEnumerable<SelectListItem> GetAllDropdownList(string obj);
    }
}
