using ProductManagement.API.Model.Dtos.Common;
using ProductManagement.API.Model.Dtos.Product;

namespace ProductManagement.API.Services.Interfaces
{
    public interface IProductService
    {
        Task<PaginationResult<ProductDto>> GetProductsAsync(ProductFilterDto filter);
        Task<ProductDto?> GetProductByIdAsync(Guid id);
        Task<bool> UpdateProductAsync(Guid id, ProductUpdateDto dto);
        Task<ProductDto> CreateProductAsync(ProductCreateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
