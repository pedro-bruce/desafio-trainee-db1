using ProductManagement.API.Model.Dtos.Category;
using ProductManagement.API.Model.Dtos.Common;

namespace ProductManagement.API.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<PaginationResult<CategoryDto>> GetAsync(CategoryFilterDto filter);
        Task<CategoryDto?> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(Guid id, CategoryUpdateDto dto);
        Task<CategoryDto> CreateAsync(CategoryCreateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
