

using Dashboard.Application.Interfaces;
using Dashboard.Application.ViewModels;
using Dashboard.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _repo;

        public ProductService(IGenericRepository<Product> repo)
        {
            _repo = repo;
        }

        public async Task<ProductSearchViewModel> GetProductsAsync(string name, DateTime? dateFrom, DateTime? dateTo)
        {
            var query = _repo.GetQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.Name.Contains(name));

            if (dateFrom.HasValue)
                query = query.Where(p => p.CreatedAt >= dateFrom.Value);

            if (dateTo.HasValue)
                query = query.Where(p => p.CreatedAt <= dateTo.Value);

            return new ProductSearchViewModel
            {
                Products = await query.ToListAsync(),
                Name = name,
                DateFrom = dateFrom,
                DateTo = dateTo
            };
        }

        public async Task<Product> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task CreateProductAsync(Product product, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                product.ImagePath = await SaveImageAsync(imageFile);
            }

            product.CreatedAt = DateTime.Now;
            await _repo.AddAsync(product);
            await _repo.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                
                DeleteImage(product.ImagePath);
                product.ImagePath = await SaveImageAsync(imageFile);
            }

            _repo.Update(product);
            await _repo.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product != null)
            {
                DeleteImage(product.ImagePath);
                _repo.Delete(product);
                await _repo.SaveChangesAsync();
            }
        }

     
        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
            return "/images/" + fileName;
        }

        private void DeleteImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) return;
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath.TrimStart('/'));
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }
    }
}