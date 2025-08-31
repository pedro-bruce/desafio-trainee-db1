using Microsoft.EntityFrameworkCore;
using ProductManagement.API.Data;
using ProductManagement.API.Model;
using ProductManagement.API.Repository.Interfaces;

namespace ProductManagement.API.Repository
{
    public class ExportRequestRepository : IExportRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public ExportRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ExportRequest> AddAsync(ExportRequest request)
        {
            _context.ExportRequests.Add(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task<ExportRequest?> GetByIdAsync(Guid id)
        {
            var request = await _context.ExportRequests.FirstOrDefaultAsync(e => e.Id == id);
            return request;
        }
    }
}
