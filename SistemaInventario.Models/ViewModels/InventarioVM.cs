using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaInventario.Models.ViewModels
{
    public class InventarioVM
    {
        public Inventario Inventario { get; set; }
        public InventarioDetalle InventarioDetalle { get; set; }
        public IEnumerable<InventarioDetalle> InventarioDetalles { get; set; }//evita la conversión implicita
        public IEnumerable<SelectListItem> BodegaList { get; set; }


    }
}
