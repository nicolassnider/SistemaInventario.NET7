using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaInventario.Models
{
    public class KardexInventario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int BodegaProductoId { get; set; }
        [ForeignKey("BodegaProductoId")]
        public BodegaProducto BodegaProducto { get; set; }
        [Required]
        [MaxLength(100)]
        public string Tipo { get; set; }//entrada o salida
        [Required]
        public string Detalle { get; set; }
        [Required]
        public int StockAnterior { get; set; }
        [Required]
        public int Cantidad { get; set; }
        [Required]
        public double Costo { get; set; }
        [Required]
        public int Stock { get; set; }
        [Required]
        public double Total { get; set; }
        [Required]
        public string UsuarioAplicacionId { get; set; }
        [ForeignKey("UsuarioAplicacionId")]
        public UsuarioAplicacion UsuarioAplicacion { get; set; }
        [Required]
        public DateTime FechaRegistro { get; set; }

    }
}
