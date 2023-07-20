using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    internal class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoriaRepository(ApplicationDbContext db) : base(db) /*envía a clase padre*/
        {
            _db = db;
        }

        public void Update(Categoria categoria)
        {
            var categoriaBD = _db.Categorias.FirstOrDefault(b => b.Id == categoria.Id);
            if (categoriaBD != null)
            {
                categoriaBD.Nombre = categoria.Nombre;
                categoriaBD.Descripcion = categoria.Descripcion;
                categoriaBD.Estado = categoria.Estado;
                _db.SaveChanges();
            }

        }
    }
}
