using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProductManagement.API.Data;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting microservice...");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var queueName = "product/export.data";
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using var conn = await factory.CreateConnectionAsync();
        using var channel = await conn.CreateChannelAsync();

        await channel.QueueDeclareAsync(queueName, false, false, false, null);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Message: {message}");

            if (Guid.TryParse(message, out var id))
            {
                Console.WriteLine("Entrou");
                await ExportProductAsync(id, connectionString);
            }
        };

        await channel.BasicConsumeAsync(queueName, true, consumer);
        Console.ReadLine();
    }

    static async Task ExportProductAsync(Guid id, string connectionString)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(connectionString)
            .Options;
        using var dbContext = new ApplicationDbContext(options);

        var product = await dbContext.Products.FindAsync(id);

        if (product != null)
        {
            Console.WriteLine("Product found.");
            var json = System.Text.Json.JsonSerializer.Serialize(product);
            var exportDir = Path.Combine(AppContext.BaseDirectory, "Exports");
            Directory.CreateDirectory(exportDir);

            var file = Path.Combine(exportDir, $"export_{product.Id}_{DateTime.Now:yyyyMMdd_HHmmss}.json");
            await File.WriteAllTextAsync(file, json);
            return;
        }

        Console.WriteLine("Product not found.");
    }
}