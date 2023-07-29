using SistemaInventario.Models;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface ICarroComprasRepository:IRepository<CarroCompras>
    {
        void Update(CarroCompras carroCompras);
    }
}
