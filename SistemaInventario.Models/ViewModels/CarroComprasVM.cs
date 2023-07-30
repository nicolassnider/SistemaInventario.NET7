using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Models.ViewModels
{
    public class CarroComprasVM
    {
        public Compania Compania { get; set; }
        public Producto Producto { get; set; }
        public int Stock { get; set; }
        public CarroCompras CarroCompras { get; set; }
        public IEnumerable<CarroCompras> CarroCompraLista { get; set; }
        public Orden Orden { get; set; }

    }
}
