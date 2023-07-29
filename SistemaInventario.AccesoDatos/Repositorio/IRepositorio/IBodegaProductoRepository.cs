using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaInventario.Models;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface IBodegaProductoRepository : IRepository<BodegaProducto>
    {
        void Update(BodegaProducto bodegaProducto);
        IEnumerable<SelectList> SelectAllDropdownList(string obj);
    }
}
