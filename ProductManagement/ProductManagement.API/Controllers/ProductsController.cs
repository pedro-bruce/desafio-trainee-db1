using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.API.Data;
using ProductManagement.API.Model;
using ProductManagement.API.Model.Dtos.Common;
using ProductManagement.API.Model.Dtos.Product;
using ProductManagement.API.Services.Interfaces;

namespace ProductManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
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
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var product = await _productService.UpdateAsync(id, dto);

            if (!product)
            {
                return NotFound();
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
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await _productService.DeleteAsync(id);

            if (!product)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost("{id}/export")]
        public async Task<IActionResult> ExportProduct(Guid id)
        {
            try
            {
                await _productService.PublishExportRequestAsync(id);
                return Accepted(new { Message = "Solicitação de exportação enviada com sucesso." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao enviar solicitação de exportação: {ex.Message}");
            }
        }
    }
}
