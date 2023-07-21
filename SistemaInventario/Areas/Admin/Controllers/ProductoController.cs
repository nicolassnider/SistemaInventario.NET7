﻿using Microsoft.AspNetCore.Mvc;
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
        public ProductoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                MarcaLista = _unitOfWork.Producto.GetAllDropdownList("Marca")
            };
            if (id==null)
            {
                //crear nuevo producto
                return View(productoVM);
            }
            else
            {
                productoVM.Producto= await _unitOfWork.Producto.Get(id.GetValueOrDefault());
                if (productoVM.Producto == null) return NotFound("Producto no encontrado");
                return View();
            }
            

        }

        #region API


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productos = await _unitOfWork.Marca.GetAll(includeProperties:"Categoria,Marca");//para traer las propiedades
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
