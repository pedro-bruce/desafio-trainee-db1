using ProductManagement.API.Model.Dtos.Category;

namespace ProductManagement.API.Model.Dtos.Product
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public CategoryDto? Category { get; set; }
    }
}
