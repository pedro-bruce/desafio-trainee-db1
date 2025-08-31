using ProductManagement.API.Model;
using ProductManagement.API.Model.Dtos.Category;
using ProductManagement.API.Model.Dtos.Common;
using ProductManagement.API.Repository.Interfaces;
using ProductManagement.API.Services.Interfaces;

namespace ProductManagement.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto> CreateAsync(CategoryCreateDto dto)
        {
            var category = new Category { Name = dto.Name };
            
            await _categoryRepository.CreateAsync(category);

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _categoryRepository.DeleteAsync(id);
        }

        public async Task<PaginationResult<CategoryDto>> GetAsync(CategoryFilterDto filter)
        {
            var (categories, totalCount) = await _categoryRepository.GetAsync(filter);

            var dtos = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();

            return new PaginationResult<CategoryDto>
            {
                Items = dtos,
                Page = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<CategoryDto?> GetByIdAsync(Guid id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(Guid id, CategoryUpdateDto dto)
        {
            return await _categoryRepository.UpdateAsync(id, dto);
        }
    }
}
