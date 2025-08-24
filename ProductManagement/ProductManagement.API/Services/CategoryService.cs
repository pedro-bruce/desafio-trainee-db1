using Microsoft.EntityFrameworkCore;
using ProductManagement.API.Data;
using ProductManagement.API.Model;
using ProductManagement.API.Model.Dtos.Category;
using ProductManagement.API.Model.Dtos.Common;
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

        public async Task<CategoryDto> CreateAsync(CategoryCreateDto dto)
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

        public async Task<PaginationResult<CategoryDto>> GetAsync(CategoryFilterDto filter)
        {
            var query = _context.Categories
                .AsQueryable()
                .Where(c => c.IsActive);

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(c => c.Name.Contains(filter.Name));
            }

            var totalCount = await query.CountAsync();

            var categories = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();

            return new PaginationResult<CategoryDto>
            {
                Items = categories,
                Page = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<CategoryDto?> GetByIdAsync(Guid id)
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

        public async Task<bool> UpdateAsync(Guid id, CategoryUpdateDto dto)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return false;
            }

            category.Name = dto.Name;
            category.ModifiedAt = DateTime.UtcNow;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
