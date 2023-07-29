namespace SistemaInventario.Models.Especificaciones
{
    public class PagedList<T>: List<T>
    {
        public MetaData MetaData { get; set; }
        public PagedList(List<T> items, int count, int PageNumber, int PageSize)
        {
            MetaData = new MetaData
            {
                TotalCount = count,
                PageSize = PageSize,
                TotalPages = (int)Math.Ceiling(count / (double)PageSize) //1.5 > 2
            };
            AddRange(items);//agrega los elementos al final de la lista
            
        }
        public static PagedList<T> ToPagedList(IEnumerable<T> entidad,int pageNumber, int pageSize) {
            var count = entidad.Count();
            var items = entidad.Skip((pageNumber-1)*pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
