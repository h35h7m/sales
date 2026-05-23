using Dashboard.Domain.Entities;

namespace Dashboard.Application.ViewModels
{
    public class ProductSearchViewModel
    {
        public string? Name { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public List<Product> Products { get; set; } = new();
    }
}
