using SistemaInventario.Models.Especificaciones;
using System.Linq.Expressions;

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
