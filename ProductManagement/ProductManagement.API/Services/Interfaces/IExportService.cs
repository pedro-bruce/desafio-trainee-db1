using ProductManagement.API.Model;

namespace ProductManagement.API.Services.Interfaces
{
    public interface IExportService
    {
        Task<Guid> PublishExportRequestAsync(Guid id);
        Task<ExportRequest?> GetExportAsync(Guid id); 
    }
}
