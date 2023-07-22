using SistemaInventario.Models.Especificaciones;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface IRepository<T> where T : class
    {
        Task<T> Get(int id);
        Task<IEnumerable<T>> GetAll(
            Expression<Func<T,bool>> filter=null,
            Func<IQueryable<T>,IOrderedQueryable<T>>orderBy=null,
            string includeProperties = null,
            bool isTracking = true
            );
        PagedList<T> GetAllPaginated(
            Parametros parametros, 
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null,
            bool isTracking = true
            );
        Task<T> GetFirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null,
            bool isTracking = true
            );

        Task Add(T entity);

        //no pueden ser async
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
