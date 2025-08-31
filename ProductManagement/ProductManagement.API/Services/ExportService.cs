using ProductManagement.API.Model;
using ProductManagement.API.Model.Enums;
using ProductManagement.API.Repository.Interfaces;
using ProductManagement.API.Services.Interfaces;

namespace ProductManagement.API.Services
{
    public class ExportService : IExportService
    {
        private readonly IExportPublisher _publisher;
        private readonly IExportRequestRepository _repository;

        public ExportService(IExportRequestRepository repository, IExportPublisher publisher)
        {
            _repository = repository;
            _publisher = publisher;
        }

        public async Task<ExportRequest?> GetExportAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Guid> PublishExportRequestAsync(Guid id)
        {
            var request = new ExportRequest
            {
                ProductId = id,
                Status = ExportStatus.Pending
            };

            await _repository.AddAsync(request);
            await _publisher.PublishAsync(request.Id);

            return request.Id;
        }
    }
}
