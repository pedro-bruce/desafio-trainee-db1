namespace ProductManagement.API.Model.Dtos.Product
{
    public class ProductCreateDto
    {
        public string? Name { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
