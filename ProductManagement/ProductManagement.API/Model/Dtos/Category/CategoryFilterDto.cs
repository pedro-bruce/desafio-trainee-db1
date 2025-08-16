using ProductManagement.API.Model.Dtos.Common;

namespace ProductManagement.API.Model.Dtos.Category
{
    public class CategoryFilterDto : PaginationParams
    {
        public string? Name { get; set; }
    }
}
