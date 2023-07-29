using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Models;
using SistemaInventario.Models.ErrorViewModels;
using SistemaInventario.Models.Especificaciones;
using SistemaInventario.Models.ViewModels;
using System.Diagnostics;

namespace SistemaInventario.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public CarroComprasVM carroComprasVM { get; set; }

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int pageNumber = 1, string busqueda="", string busquedaActual="")
        {
            if (!String.IsNullOrEmpty(busqueda))
            {
                pageNumber = 1;
            }else
            {
                busqueda = busquedaActual;
            }
            ViewData["BusquedaActiañ"] = busqueda;

            if (pageNumber <= 1) { pageNumber = 1; }

            Parametros parametros = new Parametros()
            {
                PageNumber = pageNumber,
                PageSize = 4
            };
            var result = _unitOfWork.Producto.GetAllPaginated(parametros);

            if (!String.IsNullOrEmpty(busqueda))
            {
                result = _unitOfWork.Producto.GetAllPaginated(parametros, p => p.Descripcion.ToLower().Contains(busqueda.ToLower()));
            }
            ViewData["TotalPaginas"] = result.MetaData.TotalPages;
            ViewData["TotalRegistros"] = result.MetaData.TotalCount;
            ViewData["PageSize"] = result.MetaData.PageSize;
            ViewData["PageNumber"] = pageNumber;
            ViewData["Previo"] = "disabled"; // clase para desactivar botón
            ViewData["Siguiente"] = "";

            if (pageNumber > 1) { ViewData["Previo"] = ""; }
            if (result.MetaData.TotalPages <= pageNumber) { ViewData["Siguiente"] = "disabled"; }
            return View(result);

        }
        public async Task<IActionResult> Detalle(int id)
        {
            carroComprasVM = new CarroComprasVM();
            carroComprasVM.Compania =await _unitOfWork.Compania.GetFirstOrDefault();
            carroComprasVM.Producto = await _unitOfWork.Producto
                .GetFirstOrDefault(p => p.Id == id,
                                   includeProperties:"Marca,Categoria");
            var bodegaProducto = await _unitOfWork.BodegaProducto
                .GetFirstOrDefault(b=>b.ProductoId==id && 
                                   b.BodegaID==carroComprasVM.Compania.BodegaVentaId);
            if (bodegaProducto != null)
            {
                carroComprasVM.Stock = 0;
            }
            else
            {
                carroComprasVM.Stock = bodegaProducto.Cantidad;
            }
            carroComprasVM.CarroCompras = new CarroCompras()
            {
                Producto = carroComprasVM.Producto,
                ProductoId = carroComprasVM.Producto.Id
            };
            return View(carroComprasVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}