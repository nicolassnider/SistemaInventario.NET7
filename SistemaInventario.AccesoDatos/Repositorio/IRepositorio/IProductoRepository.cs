using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaInventario.Models;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface IProductoRepository : IRepository<Producto>
    {
        void Update(Producto producto);
        IEnumerable<SelectListItem> GetAllDropdownList(string obj);
    }
}
