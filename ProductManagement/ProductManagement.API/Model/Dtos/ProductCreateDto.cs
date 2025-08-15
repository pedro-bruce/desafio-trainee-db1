namespace ProductManagement.API.Model.Dtos
{
    public class ProductCreateDto
    {
        public string? Name { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
