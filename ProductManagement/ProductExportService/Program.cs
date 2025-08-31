using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProductManagement.API.Data;
using ProductManagement.API.Model;
using ProductManagement.API.Model.Dtos.Category;
using ProductManagement.API.Model.Dtos.Product;
using ProductManagement.API.Model.Enums;
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

        var exportRequest = await dbContext.ExportRequests.FindAsync(id);

        if (exportRequest == null)
        {
            Console.WriteLine("Request not found.");
            return;
        }

        try
        {
            var product = await dbContext.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == exportRequest.ProductId);

            if (product == null)
            {
                exportRequest.Status = ExportStatus.Failed;
                exportRequest.Message = "Produto não encontrado.";
                await UpdateDbContext(exportRequest, dbContext);
                return;
            }

            if (product.WasExported)
            {
                exportRequest.Status = ExportStatus.AlreadyExported;
                exportRequest.Message = "Produto já exportado.";
                await UpdateDbContext(exportRequest, dbContext);
                return;
            }

            var productDto = new ProductExportDto
            {
                Id = product.Id,
                Name = product.Name,
                Category = product.Category?.Name
            };

            var json = System.Text.Json.JsonSerializer.Serialize(productDto);
            var exportDir = Path.Combine(AppContext.BaseDirectory, "Exports");
            Directory.CreateDirectory(exportDir);

            var file = Path.Combine(exportDir, $"{productDto.Id}_{DateTime.Now:yyyyMMdd_HHmmss}.json");
            await File.WriteAllTextAsync(file, json);

            exportRequest.Status = ExportStatus.Success;
            exportRequest.Message = "Produto exportado com sucesso.";

            product.WasExported = true;
            product.ExportedAt = DateTime.UtcNow;
            dbContext.Products.Update(product);

        }
        catch (Exception ex)
        {
            exportRequest.Status = ExportStatus.Failed;
            exportRequest.Message = ex.Message;
        }

        await UpdateDbContext(exportRequest, dbContext);
    }

    static async Task UpdateDbContext(ExportRequest exportRequest, ApplicationDbContext dbContext)
    {
        exportRequest.UpdatedAt = DateTime.UtcNow;
        dbContext.ExportRequests.Update(exportRequest);
        await dbContext.SaveChangesAsync();
    }
}