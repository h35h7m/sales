
using Dashboard.Application.ViewModels;
using Dashboard.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Dashboard.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductSearchViewModel> GetProductsAsync(string name, DateTime? dateFrom, DateTime? dateTo);
        Task<Product> GetByIdAsync(int id);
        Task CreateProductAsync(Product product, IFormFile imageFile);
        Task UpdateProductAsync(Product product, IFormFile? imageFile);
        Task DeleteProductAsync(int id);
    }
}