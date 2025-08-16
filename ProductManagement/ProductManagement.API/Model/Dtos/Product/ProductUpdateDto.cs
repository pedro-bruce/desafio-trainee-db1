namespace ProductManagement.API.Model.Dtos.Product
{
    public class ProductUpdateDto
    {
        public string? Name { get; set; }
        public Guid CategoryId { get; set; }
    }
}
