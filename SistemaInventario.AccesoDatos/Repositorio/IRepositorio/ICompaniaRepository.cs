using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaInventario.Models;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface ICompaniaRepository:IRepository<Compania>
    {
        void Update(Compania compania);
        IEnumerable<SelectListItem> GetAllDropdownList(string obj);
    }
}
