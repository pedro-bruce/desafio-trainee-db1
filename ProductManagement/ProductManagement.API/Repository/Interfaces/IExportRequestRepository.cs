using ProductManagement.API.Model;

namespace ProductManagement.API.Repository.Interfaces
{
    public interface IExportRequestRepository
    {
        Task<ExportRequest> AddAsync(ExportRequest request);
        Task<ExportRequest?> GetByIdAsync(Guid id);
    }
}
