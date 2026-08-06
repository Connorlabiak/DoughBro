using DoughBro.src.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoughBro.src.Controllers
{
    [ApiController]
    [Route("api/categories")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private static readonly IReadOnlyList<CategoryDto> Categories = new List<CategoryDto>
        {
            new() { Id = "food", Name = "Food", Color = "#ef4444" },
            new() { Id = "groceries", Name = "Groceries", Color = "#22c55e" },
            new() { Id = "transport", Name = "Transport", Color = "#0ea5e9" },
            new() { Id = "bills", Name = "Bills", Color = "#f59e0b" },
            new() { Id = "shopping", Name = "Shopping", Color = "#a855f7" },
            new() { Id = "entertainment", Name = "Entertainment", Color = "#ec4899" },
            new() { Id = "health", Name = "Health", Color = "#14b8a6" },
            new() { Id = "travel", Name = "Travel", Color = "#6366f1" },
        };

        [HttpGet]
        public ActionResult<IEnumerable<CategoryDto>> GetCategories()
        {
            return Ok(Categories);
        }
    }
}
