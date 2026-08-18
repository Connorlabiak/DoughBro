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

        /// <summary>
        /// Updates a category name for the authenticated user.
        /// </summary>
        /// <param name="userId">The authenticated Firebase user ID.</param>
        /// <param name="categoryId">The category document ID.</param>
        /// <param name="request">The requested category changes.</param>
        /// <returns>The updated category, or null if it does not exist.</returns>
        Task<CategoryDto?> UpdateCategoryAsync(string userId, string categoryId, UpdateCategoryRequest request);

        /// <summary>
        /// Deletes a category and removes it from all associated transactions.
        /// </summary>
        /// <param name="userId">The authenticated Firebase user ID.</param>
        /// <param name="categoryId">The category document ID.</param>
        /// <returns>True when the category was deleted; otherwise, false.</returns>
        Task<bool> DeleteCategoryAsync(string userId, string categoryId);
    }
}
