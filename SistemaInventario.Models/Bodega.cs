using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Models
{
    public class Bodega
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Nombre requerido")]
        [MaxLength(60,ErrorMessageResourceName ="Maximo 60 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Descripcion requerido")]
        [MaxLength(100, ErrorMessageResourceName = "Maximo 60 caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Estado requerido")]
        public bool Estado { get; set; }
    }
}
