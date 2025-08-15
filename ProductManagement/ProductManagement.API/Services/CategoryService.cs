using Microsoft.EntityFrameworkCore;
using ProductManagement.API.Data;
using ProductManagement.API.Model;
using ProductManagement.API.Model.Dtos;
using ProductManagement.API.Services.Interfaces;

namespace ProductManagement.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto dto)
        {
            var category = new Category { Name = dto.Name };
            
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
            };
        }

        /* ajustar para deletar os produtos relacionados à categoria */
        public async Task<bool> DeleteAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return false;
            }

            category.IsActive = false;
            _context.Categories.Update(category);

            var products = await _context.Products
                .Where(p => p.CategoryId == id)
                .ToListAsync();

            foreach (var product in products)
            {
                product.CategoryId = null;
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            return await _context.Categories
                .Where(c => c.IsActive)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(Guid id)
        {
            return await _context.Categories
                .Where(c => c.Id == id && c.IsActive)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .FirstOrDefaultAsync();

        }

        public async Task<bool> UpdateCategoryAsync(Guid id, CategoryUpdateDto dto)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return false;
            }

            category.Name = dto.Name;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
