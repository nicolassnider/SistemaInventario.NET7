using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using SistemaInventario.AccesoDatos.Migrations;
using SistemaInventario.AccesoDatos.Repositorio;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Models;
using SistemaInventario.Models.ViewModels;
using SistemaInventario.Utilidades;

namespace SistemaInventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _webHostEnvironment;
        public ProductoController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;   
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            ProductoVM productoVM = new ProductoVM()
            {
                Producto = new Producto(),
                CategoriaLista = _unitOfWork.Producto.GetAllDropdownList("Categoria"),
                MarcaLista = _unitOfWork.Producto.GetAllDropdownList("Marca"),
                PadreLista = _unitOfWork.Producto.GetAllDropdownList("Producto")
            };
            if (id==null)
            {
                //crear nuevo producto
                productoVM.Producto.Estado = true;
                return View(productoVM);
            }
            else
            {
                productoVM.Producto= await _unitOfWork.Producto.Get(id.GetValueOrDefault());
                if (productoVM.Producto == null) return NotFound("Producto no encontrado");
                return View(productoVM);
            }

            
            

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upsert(ProductoVM productoVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                if (productoVM.Producto.Id==0)
                {
                    //Crear
                    string upload = webRootPath + DS.ImagenRuta;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (
                        var fileStream = 
                        new FileStream(Path.Combine(upload,fileName+extension),FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productoVM.Producto.ImagenUrl = fileName + extension;
                    await _unitOfWork.Producto.Add(productoVM.Producto);

                }
                else
                {
                    //actualizar
                    var objProducto = await _unitOfWork.Producto.GetFirstOrDefault(p => p.Id == productoVM.Producto.Id, isTracking:false);
                    if (files.Count>0 ) // se selecciono una nueva imagen 
                    {
                        string upload = webRootPath+DS.ImagenRuta;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);
                        //borrar imagen anterior
                        var prevFile = Path.Combine(upload, objProducto.ImagenUrl);
                        if (System.IO.File.Exists(prevFile))
                        {
                            System.IO.File.Delete(prevFile);
                        }
                        using(var fileStream = new FileStream(Path.Combine(upload,fileName+extension),FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        productoVM.Producto.ImagenUrl= fileName + extension;
                    }
                    else
                    {
                        productoVM.Producto.ImagenUrl= objProducto.ImagenUrl;
                    }
                    _unitOfWork.Producto.Update(productoVM.Producto);

                }
                TempData[DS.Exitosa] = "Transaccion exitosa";
                await _unitOfWork.Save();
                return View("Index");
            }
            productoVM.CategoriaLista = _unitOfWork.Producto.GetAllDropdownList("Categoria");
            productoVM.MarcaLista = _unitOfWork.Producto.GetAllDropdownList("Marca");
            productoVM.PadreLista = _unitOfWork.Producto.GetAllDropdownList("Producto");
            return View(productoVM);
        }
        
        #region API


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productos = await _unitOfWork.Producto.GetAll(includeProperties:"Marca,Categoria");//para traer las propiedades
            return Json(new { data = productos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var productoDb = await _unitOfWork.Producto.Get(id);
            if (productoDb == null)
            {
                return Json(new
                { success = false, message = "Error al borrar producto" });
            }
            string upload = _webHostEnvironment.WebRootPath + DS.ImagenRuta;
            var prevFile = Path.Combine(upload, productoDb.ImagenUrl);
            if (System.IO.File.Exists(prevFile))
            {
                System.IO.File.Delete(prevFile);
            }
            _unitOfWork.Producto.Remove(productoDb);
            await _unitOfWork.Save();
            return Json(new { succes = true, message = "Producto eliminado correctamente" });
        }
        [ActionName("ValidarNumeroSerie")]
        public async Task<IActionResult> ValidarNombre(string numeroSerie, int id = 0)
        {
            bool valor = false;
            var lista = await _unitOfWork.Producto.GetAll();
            if (id == 0)
            {
                valor = lista.Any(b => b.NumeroSerie.Trim().ToLower() == numeroSerie.Trim().ToLower());
            }
            else
            {
                valor = lista.Any(b => b.NumeroSerie.Trim().ToLower() == numeroSerie.Trim().ToLower() && b.Id != id);
            }
            if (valor)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });
        }

        #endregion
    }
}
