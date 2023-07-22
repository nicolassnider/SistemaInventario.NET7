using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Models
{
    public class UsuarioAplicacion:IdentityUser
    {
        [Required(ErrorMessage ="Nombres requerido")]
        [MaxLength(80,ErrorMessage ="Maximo 80 caracteres")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Apellidos requerido")]
        [MaxLength(80, ErrorMessage = "Maximo 80 caracteres")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Direccion requerido")]
        [MaxLength(200, ErrorMessage = "Maximo 200 caracteres")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Ciudad requerido")]
        [MaxLength(60, ErrorMessage = "Maximo 60 caracteres")]
        public string Ciudad { get; set; }

        [Required(ErrorMessage = "País requerido")]
        [MaxLength(80, ErrorMessage = "Maximo 60 caracteres")]
        public string Pais { get; set; }
        [NotMapped] //no se agrega a la tabla
        public string Role { get; set; }



    }
}
