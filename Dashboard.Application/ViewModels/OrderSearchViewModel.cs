using Dashboard.Domain.Entities;

namespace Dashboard.Application.ViewModels
{
    public class OrderSearchViewModel
    {
        
        public string? CustomerName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
