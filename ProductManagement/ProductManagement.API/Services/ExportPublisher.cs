using ProductManagement.API.Repository.Interfaces;
using ProductManagement.API.Services.Interfaces;
using RabbitMQ.Client;

namespace ProductManagement.API.Services
{
    public class ExportPublisher : IExportPublisher
    {
        private readonly string _queueName = "product/export.data";
        private readonly string _hostName = "localhost";
        
        public async Task PublishAsync(Guid id)
        {
            var factory = new ConnectionFactory() { HostName = _hostName };
            var messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(id.ToString());
            using var conn = await factory.CreateConnectionAsync();
            using var channel = await conn.CreateChannelAsync();

            await channel.QueueDeclareAsync(_queueName, false, false, false, null);
            await channel.BasicPublishAsync(string.Empty, _queueName, messageBodyBytes);
        }
    }
}
