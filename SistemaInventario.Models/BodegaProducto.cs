﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Models
{
    public class BodegaProducto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int BodegaID { get; set; }
        [ForeignKey("BodegaID")]
        public Bodega Bodega { get; set; }
        [Required]
        public int ProductoId { get; set; }
        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; }
        [Required]
        public int Cantidad { get; set; }

    }
}