using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface IUnitOfWork : IDisposable
    {
        IBodegaRepository Bodega { get; }
        ICategoriaRepository Categoria { get; }
        IMarcaRepository Marca { get; }
        IProductoRepository Producto { get; }
        IUsuarioAplicacionRepositorio UsuarioAplicacion { get; }
        IBodegaProductoRepository BodegaProducto { get; }
        IInventarioRepository Inventario {get;}
        IInventarioDetalleRepository InventarioDetalle { get; }
        IKardexInventarioRepository KardexInventario { get; }
        Task Save();
    }
}
