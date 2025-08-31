using ProductManagement.API.Model;
using ProductManagement.API.Model.Dtos.Category;

namespace ProductManagement.API.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        Task<CategoryDto?> GetByIdAsync(Guid id);
        Task<(List<Category> Items, int TotalCount)> GetAsync(CategoryFilterDto filter);
        Task CreateAsync(Category category);
        Task<bool> UpdateAsync(Guid id, CategoryUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
