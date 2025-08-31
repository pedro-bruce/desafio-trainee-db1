using Microsoft.CodeAnalysis;
using ProductManagement.API.Model;
using ProductManagement.API.Model.Dtos.Category;
using ProductManagement.API.Model.Dtos.Common;
using ProductManagement.API.Model.Dtos.Product;
using ProductManagement.API.Repository.Interfaces;
using ProductManagement.API.Services.Interfaces;

namespace ProductManagement.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> CreateAsync(ProductCreateDto dto)
        {
            Category? category = null;

            if (dto.CategoryId.HasValue)
            {
                category = await _productRepository.GetCategoryByIdAsync(dto.CategoryId.Value);

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

            await _productRepository.CreateAsync(product);

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
            return await _productRepository.DeleteAsync(id);
        }

        public async Task<ProductDto?> GetByIdAsync(Guid id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<PaginationResult<ProductDto>> GetAsync(ProductFilterDto filter)
        {
            var (products, totalCount) = await _productRepository.GetAsync(filter);

            var dtos = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Category = p.Category == null ? null : new CategoryDto
                {
                    Id = p.Category.Id,
                    Name = p.Category.Name
                }
            }).ToList();

            return new PaginationResult<ProductDto>
            {
                Items = dtos,
                Page = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<bool> UpdateAsync(Guid id, ProductUpdateDto dto)
        {
            return await _productRepository.UpdateAsync(id, dto);
        }
    }
}
