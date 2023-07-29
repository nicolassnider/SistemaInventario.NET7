using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaInventario.Models
{
    public class Inventario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UsuarioAplicacionId { get;set; }
        [ForeignKey("UsuarioAplicacionId")]
        public UsuarioAplicacion UsuarioAplicacion { get; set; }
        [Required]
        public DateTime FechaInicial { get; set; }
        [Required]
        public DateTime FechaFinal { get; set; }
        [Required]
        public int BodegaId { get; set; }
        [ForeignKey("BodegaId")]
        public Bodega Bodega { get; set; }
        [Required]
        public bool Estado { get; set; }
    }
}
