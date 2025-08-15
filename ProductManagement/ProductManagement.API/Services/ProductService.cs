using Microsoft.EntityFrameworkCore;
using ProductManagement.API.Data;
using ProductManagement.API.Model;
using ProductManagement.API.Model.Dtos;
using ProductManagement.API.Services.Interfaces;

namespace ProductManagement.API.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDto> CreateProductAsync(ProductCreateDto dto)
        {
            Category? category = null;

            if (dto.CategoryId.HasValue)
            {
                category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == dto.CategoryId.Value && c.IsActive);

                if (category == null)
                {
                    throw new ArgumentException("Categoria não encontrada.");
                }
            }

            var product = new Product
            {
                Name = dto.Name,
                CategoryId = dto.CategoryId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Category = category == null ? null : new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name
                }
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return false;
            }

            product.IsActive = false;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ProductDto?> GetProductByIdAsync(Guid id)
        {
            return await _context.Products
                .Where(p => p.Id == id && p.IsActive)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = p.Category == null ? null : new CategoryDto
                    {
                        Id = p.Category.Id,
                        Name = p.Category.Name
                    }
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            return await _context.Products
                .Where(p => p.IsActive)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = p.Category == null ? null : new CategoryDto
                    {
                        Id = p.Category.Id,
                        Name = p.Category.Name
                    }
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateProductAsync(Guid id, ProductUpdateDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }

            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId);
            if (!categoryExists)
            {
                throw new ArgumentException("Categoria não encontrada.");
            }

            product.Name = dto.Name;
            product.CategoryId = dto.CategoryId;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            
            return true;
        }
    }
}
