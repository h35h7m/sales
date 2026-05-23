
using Dashboard.Application.ViewModels;
using Dashboard.Domain.Entities;

namespace Dashboard.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderSearchViewModel> GetOrdersAsync(OrderSearchViewModel searchModel);
        Task<Order> GetOrderWithDetailsAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task CreateOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
    }
}