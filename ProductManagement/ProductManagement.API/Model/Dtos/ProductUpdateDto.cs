namespace ProductManagement.API.Model.Dtos
{
    public class ProductUpdateDto
    {
        public string? Name { get; set; }
        public Guid CategoryId { get; set; }
    }
}
