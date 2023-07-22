using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class ProductoRepository : Repository<Producto>, IProductoRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductoRepository(ApplicationDbContext db) : base(db) /*envía a clase padre*/
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetAllDropdownList(string obj)
        {
            if (string.IsNullOrEmpty(obj)) return null;
            if (obj == "Categoria")
            {
                return _db.Categorias
                    .Where(c=>c.Estado == true)
                    .Select(c=>new SelectListItem {
                        Text=c.Nombre,
                        Value=c.Id.ToString()
                    });
            }
            if (obj == "Marca")
            {
                return _db.Marcas
                    .Where(c => c.Estado == true)
                    .Select(c => new SelectListItem
                    {
                        Text = c.Nombre,
                        Value = c.Id.ToString()
                    });
            }
            if (obj == "Producto")
            {
                return _db.Productos
                    .Where(c => c.Estado == true)
                    .Select(c => new SelectListItem
                    {
                        Text = c.Descripcion,
                        Value = c.Id.ToString()
                    });
            }
            return null;
            
        }

        public void Update(Producto producto)
        {
            var productoBD = _db.Productos.FirstOrDefault(b => b.Id == producto.Id);
            if (productoBD != null)
            {
                productoBD.NumeroSerie = producto.NumeroSerie;
                productoBD.Descripcion = producto.Descripcion;
                productoBD.Precio = producto.Precio;
                productoBD.Costo = producto.Costo;
                if (productoBD.ImagenUrl != null) productoBD.ImagenUrl = producto.ImagenUrl;                
                productoBD.Estado = producto.Estado;
                productoBD.CategoriaId = producto.CategoriaId;
                productoBD.MarcaId = producto.MarcaId;
                productoBD.PadreId = producto.PadreId;                
                _db.SaveChanges();
            }

        }
    }
}
