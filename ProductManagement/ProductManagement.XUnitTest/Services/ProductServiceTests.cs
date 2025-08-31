using Moq;
using ProductManagement.API.Model;
using ProductManagement.API.Model.Dtos.Product;
using ProductManagement.API.Repository.Interfaces;
using ProductManagement.API.Services;

namespace ProductManagement.XUnitTest.Services
{
    public class ProductServiceTests
    {
        private readonly ProductService _service;
        private readonly Mock<IProductRepository> _repository;

        public ProductServiceTests()
        {
            _repository = new Mock<IProductRepository>();
            _service = new ProductService(_repository.Object);
        }

        [Fact(DisplayName = "Deve cadastrar um produto com categoria")]
        public async Task CreateAsync_ShouldCreateWithCategorySuccessfully()
        {
            var categoryId = Guid.NewGuid();
            var product = new ProductCreateDto { Name = "Product", CategoryId = categoryId };
            var category = new Category { Id = categoryId, Name = "Category" };

            _repository.Setup(r => r.GetCategoryByIdAsync(categoryId)).ReturnsAsync(category);
            _repository.Setup(r => r.CreateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            var result = await _service.CreateAsync(product);

            Assert.NotNull(result);
            Assert.NotNull(result.Category);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(categoryId, result.Category.Id);
        }

        [Fact(DisplayName = "Deve cadastrar um produto sem categoria")]
        public async Task CreateAsync_ShouldCreateWithoutCategorySuccessfully()
        {
            var product = new ProductCreateDto { Name = "Product" };

            _repository.Setup(r => r.CreateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            var result = await _service.CreateAsync(product);

            Assert.NotNull(result);
            Assert.Equal(product.Name, result.Name);
        }

        [Fact(DisplayName = "Deve lançar exceção quando categoria não existir")]
        public async Task CreateAsync_ShouldThrowException_WhenCategoryDoesNotExists()
        {
            var categoryId = Guid.NewGuid();
            var dto = new ProductCreateDto { Name = "Error", CategoryId = categoryId };

            _repository.Setup(r => r.GetCategoryByIdAsync(categoryId)).ReturnsAsync((Category)null);

            var result = await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(dto));

            Assert.Equal("Categoria não encontrada.", result.Message);
        }

        [Fact(DisplayName = "Deve deletar com sucesso")]
        public async Task DeleteAsync_ShouldDeleteSuccessfully()
        {
            var id = Guid.NewGuid();

            _repository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

            var result = await _service.DeleteAsync(id);

            Assert.True(result);
        }

        [Fact(DisplayName = "Deve buscar por id com sucesso")]
        public async Task GetByIdAsync_ShouldGetSuccessfully()
        {
            var id = Guid.NewGuid();
            var product = new ProductDto { Id = id, Name = "Product" };

            _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(product);

            var result = await _service.GetByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal(product, result);
        }

        [Fact(DisplayName = "Deve retornar produtos paginados")]
        public async Task GetAsync_ShouldGetProductsPaginatedSuccessfully()
        {
            var filter = new ProductFilterDto { PageNumber = 1, PageSize = 10 };

            var products = new List<Product>
            {
                new() { Id = Guid.NewGuid(), Name = "Product 1" },
                new() { Id = Guid.NewGuid(), Name = "Product 2" }
            };

            _repository.Setup(r => r.GetAsync(filter)).ReturnsAsync((products, products.Count));
            
            var result = await _service.GetAsync(filter);

            Assert.NotNull(result);
            Assert.Equal(2, result.TotalCount);
        }

        [Fact(DisplayName = "Deve atualizar produto com sucesso")]
        public async Task UpdateAsync_ShouldUpdateSuccessfully()
        {
            var id = Guid.NewGuid();
            var product = new ProductUpdateDto { Name = "Product" };

            _repository.Setup(r => r.UpdateAsync(id, product)).ReturnsAsync(true);

            var result = await _service.UpdateAsync(id, product);

            Assert.True(result);
        }
    }
}
