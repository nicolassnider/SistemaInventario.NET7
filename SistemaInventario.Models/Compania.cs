﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaInventario.Models
{
    public class Compania
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(80)]
        public string Nombre { get; set; }
        [Required]
        [MaxLength(100)]
        public string Descripcion { get; set; }
        [Required]
        [MaxLength(60)]
        public string Pais { get; set; }
        [Required]
        [MaxLength(60)]
        public string Ciudad { get; set; }
        [Required]
        [MaxLength(100)]
        public string Direccion { get; set; }
        [Required]
        [MaxLength(40)]
        public string Telefono { get; set; }
        [Required]        
        public int BodegaVentaId { get; set; }
        [ForeignKey("BodegaVentaId")]
        public Bodega Bodega { get; set; }
        public string CreadoPorId { get; set; }
        [ForeignKey("CreadoPorId")]
        public UsuarioAplicacion CreadoPor { get; set; }
        public string ActualizadoPorId { get; set; }
        [ForeignKey("ActualizadoPorId ")]
        public UsuarioAplicacion ActualizadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
