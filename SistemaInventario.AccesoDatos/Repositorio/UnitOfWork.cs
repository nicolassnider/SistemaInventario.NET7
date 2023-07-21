using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IBodegaRepository Bodega { get;private set; }
        public ICategoriaRepository Categoria { get; private set; }
        public IMarcaRepository Marca { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Bodega = new BodegaRepository(_db);
            Categoria = new CategoriaRepository(_db);
            Marca = new MarcaRepository(_db);
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
