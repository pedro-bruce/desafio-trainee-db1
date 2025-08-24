using ProductManagement.API.Model.Dtos.Common;
using ProductManagement.API.Model.Dtos.Product;

namespace ProductManagement.API.Services.Interfaces
{
    public interface IProductService
    {
        Task<PaginationResult<ProductDto>> GetAsync(ProductFilterDto filter);
        Task<ProductDto?> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(Guid id, ProductUpdateDto dto);
        Task<ProductDto> CreateAsync(ProductCreateDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task PublishExportRequestAsync(Guid id);
    }
}
