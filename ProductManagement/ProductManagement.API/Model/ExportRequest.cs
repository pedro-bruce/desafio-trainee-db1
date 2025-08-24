using ProductManagement.API.Model.Enums;

namespace ProductManagement.API.Model
{
    public class ExportRequest
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public ExportStatus Status { get; set; }
        public string? Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
