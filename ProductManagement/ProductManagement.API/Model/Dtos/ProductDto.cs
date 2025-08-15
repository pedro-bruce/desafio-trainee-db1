namespace ProductManagement.API.Model.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public CategoryDto? Category { get; set; }
    }
}
