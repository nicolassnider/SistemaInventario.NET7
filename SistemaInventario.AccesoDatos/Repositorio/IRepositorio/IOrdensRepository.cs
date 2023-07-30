using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using SistemaInventario.Models;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface IOrdenRepository:IRepository<Orden>
    {
        void Update(Orden orden);
        void UpdateEstado(int id, string ordenEstado, string pagoEstado);
        void UpdatePagoStripeId(int id, string sessionId, string transaccionId);
    }
}
