using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaInventario.Models.ViewModels
{
    public class CompaniaVM
    {
        public Compania Compania { get; set; }
        public IEnumerable<SelectListItem> BodegaLista { get; set; }
    }
}
