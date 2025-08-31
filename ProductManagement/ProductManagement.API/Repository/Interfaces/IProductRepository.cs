using ProductManagement.API.Model;
using ProductManagement.API.Model.Dtos.Category;
using ProductManagement.API.Model.Dtos.Common;
using ProductManagement.API.Model.Dtos.Product;

namespace ProductManagement.API.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<ProductDto?> GetByIdAsync(Guid id);
        Task<(List<Product> Items, int TotalCount)> GetAsync(ProductFilterDto filter);
        Task CreateAsync(Product product);
        Task<bool> UpdateAsync(Guid id, ProductUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<Category?> GetCategoryByIdAsync(Guid id);
    }
}
