using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Models
{
    public class Producto
    {
        [Key]
        public int Id{ get; set; }

        [Required(ErrorMessage = "Serie requerido")]
        [MaxLength(60, ErrorMessage = "Máximo 60 caracteres")]
        public string NumeroSerie { get; set; }

        [Required(ErrorMessage = "Descripcion requerido")]
        [MaxLength(60, ErrorMessage = "Máximo 60 caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Precio requerido")]
        public double Precio { get; set; }

        [Required(ErrorMessage = "Costo requerido")]
        public double Costo { get; set; }
        public string ImagenUrl { get; set; }

        [Required]
        public bool Estado { get; set;}

        [Required]
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }

        [Required]
        public int MarcaId { get; set; }
        [ForeignKey("MarcaId")]
        public Marca Marca { get; set; }
        
        //recursividad
        public int? PadreId { get; set; }
        public virtual Producto Padre { get; set; }
    }
}
