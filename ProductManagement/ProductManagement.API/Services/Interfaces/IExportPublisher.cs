namespace ProductManagement.API.Services.Interfaces
{
    public interface IExportPublisher
    {
        Task PublishAsync(Guid id);
    }
}
