using DoughBro.src.DTOs;
using DoughBro.src.Models;
using DoughBro.src.Repositories.Interfaces;
using DoughBro.src.Services.Interfaces;

namespace DoughBro.src.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITransactionRepository _transactionRepository;

        public CategoryService(ICategoryRepository categoryRepository, ITransactionRepository transactionRepository)
        {
            _categoryRepository = categoryRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync(string userId)
        {
            IEnumerable<CategoryModel> categories = await _categoryRepository.GetCategoriesAsync(userId);
            return categories.Select(ToDto);
        }

        public async Task<IEnumerable<CategoryColorDto>> GetCategoryColorsAsync(string userId)
        {
            ISet<string> usedColorIds = await _categoryRepository.GetUsedCategoryColorIdsAsync(userId);

            return CategoryPalette.Colors.Select(color => new CategoryColorDto
            {
                Id = color.Id,
                Name = color.Name,
                Hex = color.Hex,
                IsUsed = usedColorIds.Contains(color.Id),
            });
        }

        public async Task<CategoryDto> AddCategoryAsync(string userId, CreateCategoryRequest request)
        {
            string name = request.Name.Trim();
            string colorId = request.ColorId.Trim().ToLowerInvariant();
            CategoryColorModel? color = CategoryPalette.FindById(colorId);

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name is required.");
            }

            if (color is null)
            {
                throw new ArgumentException("Selected category color is not supported.");
            }

            ISet<string> usedColorIds = await _categoryRepository.GetUsedCategoryColorIdsAsync(userId);
            if (usedColorIds.Contains(color.Id))
            {
                throw new InvalidOperationException("Selected category color is already in use.");
            }

            CategoryModel? category = await _categoryRepository.AddCategoryAsync(userId, new CategoryModel
            {
                UserId = userId,
                Name = name,
                ColorId = color.Id,
                Color = color.Hex,
            });

            if (category is null)
            {
                throw new InvalidOperationException("Selected category color is already in use.");
            }

            return ToDto(category);
        }

        public async Task<CategoryDto?> UpdateCategoryAsync(string userId, string categoryId, UpdateCategoryRequest request)
        {
            string name = request.Name.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name is required.");
            }

            CategoryModel? category = await _categoryRepository.UpdateCategoryNameAsync(userId, categoryId, name);
            return category is null ? null : ToDto(category);
        }

        public async Task<IEnumerable<TransactionDto>> GetCategoryTransactionsAsync(string userId, string categoryId)
        {
            IEnumerable<TransactionModel> transactions = await _transactionRepository.GetTransactionsByCategoryAsync(userId, categoryId);
            return transactions.Select(transaction => new TransactionDto
            {
                Id = transaction.Id,
                Origin = transaction.Origin,
                UserId = transaction.UserId,
                Name = transaction.Name,
                Date = transaction.Date,
                Amount = transaction.Amount,
                Description = transaction.Description,
                IsPending = transaction.IsPending,
                MerchantName = transaction.MerchantName,
                Category = transaction.Category,
            });
        }

        public async Task<bool> DeleteCategoryAsync(string userId, string categoryId)
        {
            await _transactionRepository.ClearCategoryAsync(userId, categoryId);
            return await _categoryRepository.DeleteCategoryAsync(userId, categoryId);
        }

        private static CategoryDto ToDto(CategoryModel category)
        {
            return new CategoryDto
            {
                Id = category.Id!,
                Name = category.Name,
                ColorId = category.ColorId,
                Color = category.Color,
            };
        }
    }
}
