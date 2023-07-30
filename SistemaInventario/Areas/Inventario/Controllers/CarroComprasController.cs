using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Models.ViewModels;
using System.Security.Claims;

namespace SistemaInventario.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class CarroComprasController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public CarroComprasVM carroComprasVM { get; set; }

        public CarroComprasController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var claim = obtenerUsuarioId();
            carroComprasVM = new CarroComprasVM();
            carroComprasVM.Orden = new Models.Orden();
            carroComprasVM.CarroCompraLista = await _unitOfWork.CarroCompras
                .GetAll(u => u.UsuarioAplicacionId == claim.Value,
                        includeProperties:"Producto");
            carroComprasVM.Orden.TotalOrden = 0;
            carroComprasVM.Orden.UsuarioAplicacionId = claim.Value;
            foreach (var lista in carroComprasVM.CarroCompraLista)
            {
                lista.Precio = lista.Producto.Precio; //precio actual del producto
                carroComprasVM.Orden.TotalOrden += (lista.Precio * lista.Cantidad);
            }
            return View(carroComprasVM);
        }

        private Claim obtenerUsuarioId()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return claim;
        }
    }
}
