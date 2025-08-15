namespace ProductManagement.API.Model
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public virtual ICollection<Product> Products { get; set; } = [];
    }
}
