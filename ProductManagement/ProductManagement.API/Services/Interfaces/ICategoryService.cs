using ProductManagement.API.Model.Dtos.Category;
using ProductManagement.API.Model.Dtos.Common;

namespace ProductManagement.API.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<PaginationResult<CategoryDto>> GetCategoriesAsync(CategoryFilterDto filter);
        Task<CategoryDto?> GetCategoryByIdAsync(Guid id);
        Task<bool> UpdateCategoryAsync(Guid id, CategoryUpdateDto dto);
        Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
