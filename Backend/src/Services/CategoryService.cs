using DoughBro.src.DTOs;
using DoughBro.src.Models;
using DoughBro.src.Repositories.Interfaces;
using DoughBro.src.Services.Interfaces;

namespace DoughBro.src.Services
{
    public class CategoryService : ICategoryService
    {
        private static readonly (string Name, string ColorId)[] DefaultCategories =
        [
            ("Gas", "amber"),
            ("Eating Out", "teal"),
            ("Groceries", "green"),
            ("Other", "stone"),
            ("Rent", "tomato"),
            ("Entertainment", "lime"),
            ("Subscriptions", "blue"),
            ("Utilities", "yellow"),
            ("Hidden", "charcoal"),
        ];

        private readonly ICategoryRepository _categoryRepository;
        private readonly ITransactionService _transactionService;

        public CategoryService(ICategoryRepository categoryRepository, ITransactionService transactionService)
        {
            _categoryRepository = categoryRepository;
            _transactionService = transactionService;
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

        public async Task EnsureDefaultCategoriesAsync(string userId)
        {
            IEnumerable<CategoryModel> existingCategories = await _categoryRepository.GetCategoriesAsync(userId);
            if (existingCategories.Any())
            {
                return;
            }

            IEnumerable<CategoryModel> defaultCategories = DefaultCategories.Select(defaultCategory =>
            {
                CategoryColorModel color = CategoryPalette.FindById(defaultCategory.ColorId)!;
                return new CategoryModel
                {
                    UserId = userId,
                    Name = defaultCategory.Name,
                    ColorId = color.Id,
                    Color = color.Hex,
                };
            });

            await _categoryRepository.AddDefaultCategoriesAsync(userId, defaultCategories);
        }

        public async Task<CategoryDto?> UpdateCategoryAsync(string userId, string categoryId, UpdateCategoryRequest request)
        {
            string name = request.Name.Trim();
            string colorId = request.ColorId.Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name is required.");
            }

            CategoryColorModel? color = CategoryPalette.FindById(colorId);
            if (color is null)
            {
                throw new ArgumentException("Selected category color is not supported.");
            }

            IEnumerable<CategoryModel> categories = await _categoryRepository.GetCategoriesAsync(userId);
            CategoryModel? existingCategory = categories.FirstOrDefault(category => category.Id == categoryId);
            if (existingCategory is null)
            {
                return null;
            }

            ISet<string> usedColorIds = await _categoryRepository.GetUsedCategoryColorIdsAsync(userId);
            if (!string.Equals(existingCategory.ColorId, color.Id, StringComparison.OrdinalIgnoreCase) && usedColorIds.Contains(color.Id))
            {
                throw new InvalidOperationException("Selected category color is already in use.");
            }

            CategoryModel? category = await _categoryRepository.UpdateCategoryAsync(userId, categoryId, new CategoryModel
            {
                UserId = userId,
                Name = name,
                ColorId = color.Id,
                Color = color.Hex,
            });
            return category is null ? null : ToDto(category);
        }

        public async Task<bool> DeleteCategoryAsync(string userId, string categoryId)
        {
            IEnumerable<CategoryModel> categories = await _categoryRepository.GetCategoriesAsync(userId);
            if (!categories.Any(category => category.Id == categoryId))
            {
                return false;
            }

            await _transactionService.ClearCategoryAsync(userId, categoryId);
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
