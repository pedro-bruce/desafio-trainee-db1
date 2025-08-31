using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.API.Data;
using ProductManagement.API.Model;
using ProductManagement.API.Model.Dtos.Common;
using ProductManagement.API.Model.Dtos.Error;
using ProductManagement.API.Model.Dtos.Product;
using ProductManagement.API.Services.Interfaces;

namespace ProductManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IExportService _exportService;

        public ProductsController(IProductService productService, IExportService exportService)
        {
            _productService = productService;
            _exportService = exportService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResult<Product>>> GetProducts([FromQuery] ProductFilterDto filter)
        {
            var products = await _productService.GetAsync(filter);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(Guid id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(new ErrorResponse { ErrorMessage = "Produto não encontrado." });
            }

            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _productService.UpdateAsync(id, dto);

            if (!product)
            {
                return NotFound(new ErrorResponse { ErrorMessage = "Produto não encontrado." });
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] ProductCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var product = await _productService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErrorResponse { ErrorMessage = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await _productService.DeleteAsync(id);

            if (!product)
            {
                return NotFound(new ErrorResponse { ErrorMessage = "Produto não encontrado." });
            }

            return NoContent();
        }

        [HttpPost("{id}/export")]
        public async Task<IActionResult> ExportProduct(Guid id)
        {
            try
            {
                var requestId = await _exportService.PublishExportRequestAsync(id);
                return Accepted(new { Message = "Solicitação de exportação enviada com sucesso.", RequestId = requestId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao enviar solicitação de exportação: {ex.Message}");
            }
        }

        [HttpGet("/exports/{id}")]
        public async Task<IActionResult> GetExportStatus(Guid id)
        {
            var request = await _exportService.GetExportAsync(id);

            if (request == null)
            {
                return NotFound(new ErrorResponse { ErrorMessage = "Requisição não encontrada." });
            }

            return Ok(new
            {
                request.Id,
                Status = request.Status.ToString(),
                request.Message,
                request.CreatedAt,
                request.UpdatedAt
            });
        }
    }
}
