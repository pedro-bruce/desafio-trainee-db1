using Microsoft.CodeAnalysis;
using Moq;
using ProductManagement.API.Model;
using ProductManagement.API.Model.Enums;
using ProductManagement.API.Repository.Interfaces;
using ProductManagement.API.Services;
using ProductManagement.API.Services.Interfaces;

namespace ProductManagement.XUnitTest.Services
{
    public class ExportServiceTests
    {
        private readonly Mock<IExportRequestRepository> _repository;
        private readonly Mock<IExportPublisher> _publisher;
        private readonly ExportService _service;

        public ExportServiceTests()
        {
            _repository = new Mock<IExportRequestRepository>();
            _publisher = new Mock<IExportPublisher>();
            _service = new ExportService(_repository.Object, _publisher.Object);
        }

        [Fact(DisplayName = "Deve retornar exportação com sucesso")]
        public async Task GetExportAsync_ShouldReturnExportSuccessfully()
        {
            var id = Guid.NewGuid();
            var export = new ExportRequest { Id = id };

            _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(export);

            var result = await _service.GetExportAsync(id);

            Assert.NotNull(result);
            Assert.Equal(export, result);
        }

        [Fact(DisplayName = "Deve publicar exportação com sucesso")]
        public async Task PublishExportRequestAsync_ShouldPublishExportSuccessfully()
        {
            var productId = Guid.NewGuid();

            _repository.Setup(r => r.AddAsync(It.IsAny<ExportRequest>())).ReturnsAsync((ExportRequest r) =>
            {
                r.Id = Guid.NewGuid();
                return r;
            });

            _publisher.Setup(r => r.PublishAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);

            var result = await _service.PublishExportRequestAsync(productId);

            Assert.NotEqual(Guid.Empty, result);
        }
    }
}
