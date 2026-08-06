using DoughBro.src.DTOs;

namespace DoughBro.src.Services.Interfaces
{
    public interface ICategoryService
    {
        /// <summary>
        /// Gets categories for the authenticated user.
        /// </summary>
        /// <param name="userId">The authenticated Firebase user ID.</param>
        /// <returns>The user's category DTOs.</returns>
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync(string userId);

        /// <summary>
        /// Gets the standardized category color palette with user usage flags.
        /// </summary>
        /// <param name="userId">The authenticated Firebase user ID.</param>
        /// <returns>The category color palette.</returns>
        Task<IEnumerable<CategoryColorDto>> GetCategoryColorsAsync(string userId);

        /// <summary>
        /// Adds a category for the authenticated user.
        /// </summary>
        /// <param name="userId">The authenticated Firebase user ID.</param>
        /// <param name="request">The category creation request.</param>
        /// <returns>The created category DTO.</returns>
        Task<CategoryDto> AddCategoryAsync(string userId, CreateCategoryRequest request);
    }
}
