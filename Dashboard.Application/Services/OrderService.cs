
using Dashboard.Application.Interfaces;
using Dashboard.Application.ViewModels;
using Dashboard.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Dashboard.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepo;
        private readonly IGenericRepository<Product> _productRepo;

        public OrderService(IGenericRepository<Order> orderRepo, IGenericRepository<Product> productRepo)
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
        }

        public async Task<OrderSearchViewModel> GetOrdersAsync(OrderSearchViewModel searchModel)
        {
            var query = _orderRepo.GetQueryable();

            if (!string.IsNullOrEmpty(searchModel.CustomerName))
                query = query.Where(o => o.CustomerName.Contains(searchModel.CustomerName));

            if (searchModel.DateFrom.HasValue)
                query = query.Where(o => o.OrderDate >= searchModel.DateFrom.Value);

            if (searchModel.DateTo.HasValue)
                query = query.Where(p => p.OrderDate <= searchModel.DateTo.Value);

            searchModel.Orders = await query.OrderByDescending(o => o.OrderDate).ToListAsync();
            return searchModel;
        }

        public async Task<Order> GetOrderWithDetailsAsync(int id)
        {
            
            return await _orderRepo.GetQueryable()
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync() => await _productRepo.GetAllAsync();

        public async Task CreateOrderAsync(Order order)
        {
            order.OrderDate = DateTime.Now;
            order.TotalAmount = order.OrderDetails.Sum(d => d.UnitPrice * d.Quantity);

            await _orderRepo.AddAsync(order);
            await _orderRepo.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            
            var existingOrder = await GetOrderWithDetailsAsync(order.Id);
            if (existingOrder != null)
            {
                existingOrder.CustomerName = order.CustomerName;
                existingOrder.OrderDate = order.OrderDate;

          
                existingOrder.OrderDetails = order.OrderDetails;
                existingOrder.TotalAmount = order.OrderDetails.Sum(d => d.UnitPrice * d.Quantity);

                _orderRepo.Update(existingOrder);
                await _orderRepo.SaveChangesAsync();
            }
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order != null)
            {
                _orderRepo.Delete(order);
                await _orderRepo.SaveChangesAsync();
            }
        }
    }
}