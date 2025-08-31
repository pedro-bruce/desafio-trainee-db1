using Moq;
using ProductManagement.API.Model;
using ProductManagement.API.Model.Dtos.Category;
using ProductManagement.API.Repository.Interfaces;
using ProductManagement.API.Services;

namespace ProductManagement.XUnitTest.Services
{
    public class CategoryServiceTests
    {
        private readonly CategoryService _service;
        private readonly Mock<ICategoryRepository> _repository;

        public CategoryServiceTests()
        {
            _repository = new Mock<ICategoryRepository>();
            _service = new CategoryService(_repository.Object);
        }

        [Fact(DisplayName = "Deve criar uma categoria com sucesso")]
        public async Task CreateAsync_ShouldCreateSuccessfully()
        {
            var category = new CategoryCreateDto
            {
                Name = "Test"
            };

            var result = await _service.CreateAsync(category);

            Assert.Equal(result.Name, category.Name);
        }

        [Fact(DisplayName = "Deve deletar uma categoria com sucesso")]
        public async Task DeleteAsync_ShouldDeleteSuccessfully()
        {
            var id = Guid.NewGuid();

            _repository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

            var result = await _service.DeleteAsync(id);

            Assert.True(result);
        }

        [Fact(DisplayName = "Deve retornar categorias paginadas")]
        public async Task GetAsync_ShouldReturnPaginated()
        {
            var filter = new CategoryFilterDto { PageNumber = 1, PageSize = 2 };
            var categories = new List<Category>
            {
                new() { Id = Guid.NewGuid(), Name = "Category 1" },
                new() { Id = Guid.NewGuid(), Name = "Category 2" },
            };

            _repository.Setup(r => r.GetAsync(filter)).ReturnsAsync((categories, categories.Count));

            var result = await _service.GetAsync(filter);

            Assert.NotNull(result);
            Assert.Equal(2, result.TotalCount);
            Assert.Equal(filter.PageNumber, result.Page);
            Assert.Equal(filter.PageSize, result.PageSize);
        }

        [Fact(DisplayName = "Deve retornar categoria por Id")]
        public async Task GetByIdAsync_ShouldReturnCategorySuccessfully()
        {
            var id = Guid.NewGuid();
            var category = new CategoryDto { Id = id, Name = "Test" };

            _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(category);

            var result = await _service.GetByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal(category, result);
        }

        [Fact(DisplayName = "Deve atualizar categoria")]
        public async Task UpdateAsync_ShouldUpdateCategorySuccessfully()
        {
            var id = Guid.NewGuid();
            var dto = new CategoryUpdateDto { Name = "Test" };

            _repository.Setup(r => r.UpdateAsync(id, dto)).ReturnsAsync(true);

            var result = await _service.UpdateAsync(id, dto);

            Assert.True(result);
        }
    }
}
