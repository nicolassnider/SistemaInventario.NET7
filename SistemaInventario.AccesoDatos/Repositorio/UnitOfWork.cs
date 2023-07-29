using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IBodegaRepository Bodega { get;private set; }
        public ICategoriaRepository Categoria { get; private set; }
        public IMarcaRepository Marca { get; private set; }
        public IProductoRepository Producto { get; private set; }
        public IUsuarioAplicacionRepositorio UsuarioAplicacion { get; private set; }
        public IBodegaProductoRepository BodegaProducto { get; private set; }
        public IInventarioRepository Inventario { get; private set; }
        public IInventarioDetalleRepository InventarioDetalle { get; private set; }
        public IKardexInventarioRepository KardexInventario { get; private set; }
        public ICompaniaRepository Compania { get; private set; }
        public ICarroComprasRepository CarroCompras { get; private set; }
        public IOrdenRepository Orden { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Bodega = new BodegaRepository(_db);
            Categoria = new CategoriaRepository(_db);
            Marca = new MarcaRepository(_db);
            Producto = new ProductoRepository(_db);
            UsuarioAplicacion = new UsuarioAplicacionRepository(_db);
            BodegaProducto = new BodegaProductoRepository(_db);
            Inventario = new InventarioRepository(_db);
            InventarioDetalle = new InventarioDetalleRepository(_db);
            KardexInventario = new KardexInventarioRepository(_db);
            Compania = new CompaniaRepository(_db);
            CarroCompras = new CarroComprasRepository(_db);
            Orden = new OrdenRepository(_db);
            
        }        

        public void Dispose()
        {
            _db.Dispose();           
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
