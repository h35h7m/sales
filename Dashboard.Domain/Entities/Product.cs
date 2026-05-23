namespace Dashboard.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? ImagePath { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
