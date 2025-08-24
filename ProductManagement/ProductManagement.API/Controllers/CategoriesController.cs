using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.API.Data;
using ProductManagement.API.Model;
using ProductManagement.API.Model.Dtos.Category;
using ProductManagement.API.Model.Dtos.Common;
using ProductManagement.API.Services.Interfaces;

namespace ProductManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResult<CategoryDto>>> GetCategories([FromQuery] CategoryFilterDto filter)
        {
            var categories = await _categoryService.GetAsync(filter);
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(Guid id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(Guid id, [FromBody] CategoryUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            var category = await _categoryService.UpdateAsync(id, dto);

            if (!category)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CategoryCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var category = await _categoryService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var category = await _categoryService.DeleteAsync(id);

            if (!category)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
