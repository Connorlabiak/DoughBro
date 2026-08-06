using DoughBro.src.DTOs;
using DoughBro.src.Models;
using DoughBro.src.Repositories.Interfaces;
using DoughBro.src.Services.Interfaces;

namespace DoughBro.src.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
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
