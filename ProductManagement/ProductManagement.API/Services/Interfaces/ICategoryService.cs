using ProductManagement.API.Model.Dtos;

namespace ProductManagement.API.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(Guid id);
        Task<bool> UpdateCategoryAsync(Guid id, CategoryUpdateDto dto);
        Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
