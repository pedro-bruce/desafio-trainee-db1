using ProductManagement.API.Model.Dtos.Common;

namespace ProductManagement.API.Model.Dtos.Product
{
    public class ProductFilterDto : PaginationParams
    {
        public string? Name { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
