namespace SistemaInventario.Models.ViewModels
{
    public class OrdenDetalleVm
    {
        public Compania Compania { get; set; }
        public Orden Orden { get; set; }
        public IEnumerable<OrdenDetalle> OrdenDetalleLista { get; set; }
    }
}
