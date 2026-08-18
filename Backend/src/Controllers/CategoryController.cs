using DoughBro.src.DTOs;
using DoughBro.src.Extensions;
using DoughBro.src.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoughBro.src.Controllers
{
    [ApiController]
    [Route("api/categories")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            string? userId = User.GetUserId();
            if (userId is null)
            {
                return Unauthorized("User ID not found in claims");
            }

            IEnumerable<CategoryDto> categories = await _categoryService.GetCategoriesAsync(userId);
            return Ok(categories);
        }

        [HttpGet("colors")]
        public async Task<IActionResult> GetCategoryColors()
        {
            string? userId = User.GetUserId();
            if (userId is null)
            {
                return Unauthorized("User ID not found in claims");
            }

            IEnumerable<CategoryColorDto> colors = await _categoryService.GetCategoryColorsAsync(userId);
            return Ok(colors);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CreateCategoryRequest request)
        {
            string? userId = User.GetUserId();
            if (userId is null)
            {
                return Unauthorized("User ID not found in claims");
            }

            try
            {
                CategoryDto category = await _categoryService.AddCategoryAsync(userId, request);
                return CreatedAtAction(nameof(GetCategories), new { id = category.Id }, category);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPatch("{categoryId}")]
        public async Task<IActionResult> UpdateCategory(string categoryId, [FromBody] UpdateCategoryRequest request)
        {
            string? userId = User.GetUserId();
            if (userId is null)
            {
                return Unauthorized("User ID not found in claims");
            }

            try
            {
                CategoryDto? category = await _categoryService.UpdateCategoryAsync(userId, categoryId, request);
                return category is null ? NotFound() : Ok(category);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCategory(string categoryId)
        {
            string? userId = User.GetUserId();
            if (userId is null)
            {
                return Unauthorized("User ID not found in claims");
            }

            bool wasDeleted = await _categoryService.DeleteCategoryAsync(userId, categoryId);
            return wasDeleted ? NoContent() : NotFound();
        }
    }
}
