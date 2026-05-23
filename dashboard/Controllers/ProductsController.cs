using Dashboard.Application.Interfaces;
using Dashboard.Application.ViewModels;
using Dashboard.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dashboard.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProductService _service;

 
        public ProductsController(IProductService service)
        {
            _service = service;
        }


        public async Task<IActionResult> Index(string name, DateTime? dateFrom, DateTime? dateTo)
        {
            var model = await _service.GetProductsAsync(name, dateFrom, dateTo);
            return View(model);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateProductAsync(product, imageFile);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile? imageFile)
        {
            if (id != product.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _service.UpdateProductAsync(product, imageFile);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

 
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}