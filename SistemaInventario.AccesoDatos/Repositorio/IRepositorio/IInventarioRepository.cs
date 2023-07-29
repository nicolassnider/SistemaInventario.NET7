using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaInventario.Models;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface IInventarioRepository : IRepository<Inventario>
    {
        void Update(Inventario inventario);
        IEnumerable<SelectListItem> GetAllDropdownList(string obj);
    }
}
