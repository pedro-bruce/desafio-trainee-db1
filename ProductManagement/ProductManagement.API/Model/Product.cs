using System.Text.Json.Serialization;

namespace ProductManagement.API.Model
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public bool IsActive { get; set; } = true;
        public bool WasExported { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExportedAt { get; set; }
        public Guid? CategoryId { get; set; }
        [JsonIgnore]
        public virtual Category? Category { get; set; }
    }
}
