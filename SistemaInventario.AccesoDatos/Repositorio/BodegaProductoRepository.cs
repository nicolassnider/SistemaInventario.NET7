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
    public class BodegaProductoRepository : Repository<BodegaProducto>, IBodegaProductoRepository
    {
        private readonly ApplicationDbContext _db;

        public BodegaProductoRepository(ApplicationDbContext db):base(db)
        {
            _db = db;                
        }
        public IEnumerable<SelectList> SelectAllDropdownList(string obj)
        {
            throw new NotImplementedException();
        }

        public void Update(BodegaProducto bodegaProducto)
        {
            var bodegaProductoBD = _db.BodegasProductos.FirstOrDefault(b=>b.Id == bodegaProducto.Id);
            if (bodegaProductoBD!=null)
            {
                bodegaProductoBD.Cantidad = bodegaProducto.Cantidad;
                _db.SaveChanges();
            }
        }
    }
}
