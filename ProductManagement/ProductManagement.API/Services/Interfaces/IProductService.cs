using ProductManagement.API.Model.Dtos;

namespace ProductManagement.API.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(Guid id);
        Task<bool> UpdateProductAsync(Guid id, ProductUpdateDto dto);
        Task<ProductDto> CreateProductAsync(ProductCreateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
