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
        ICompaniaRepository Compania { get; }
        ICarroComprasRepository CarroCompras { get; }
        IOrdenRepository Orden { get; }

        Task Save();
    }
}
