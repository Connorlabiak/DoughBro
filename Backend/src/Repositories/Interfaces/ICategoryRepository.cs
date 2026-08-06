using DoughBro.src.Models;

namespace DoughBro.src.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        /// <summary>
        /// Gets categories for the authenticated user.
        /// </summary>
        /// <param name="userId">The authenticated Firebase user ID.</param>
        /// <returns>The user's category models.</returns>
        Task<IEnumerable<CategoryModel>> GetCategoriesAsync(string userId);

        /// <summary>
        /// Adds a category and reserves its color for the authenticated user.
        /// </summary>
        /// <param name="userId">The authenticated Firebase user ID.</param>
        /// <param name="category">The category model to add.</param>
        /// <returns>The saved category, or null if the color is already reserved.</returns>
        Task<CategoryModel?> AddCategoryAsync(string userId, CategoryModel category);

        /// <summary>
        /// Gets category color IDs already used by the authenticated user.
        /// </summary>
        /// <param name="userId">The authenticated Firebase user ID.</param>
        /// <returns>The user's used category color IDs.</returns>
        Task<ISet<string>> GetUsedCategoryColorIdsAsync(string userId);
    }
}
