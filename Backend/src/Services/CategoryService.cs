using DoughBro.src.DTOs;
using DoughBro.src.Models;
using DoughBro.src.Repositories.Interfaces;
using DoughBro.src.Services.Interfaces;
using System.Text.RegularExpressions;

namespace DoughBro.src.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryModel> CreateAsync(CategoryDto catgoryDto)
        {
            CategoryModel categoryModel = new CategoryModel()
            {
                Name = catgoryDto.Name.FirstCharToUpperOnly()
            };
            categoryModel.Id = await _categoryRepository.CreateAsync(categoryModel);
            return categoryModel;

        }

        public Task<CategoryModel?> FindAsync(string name)
        {
            return _categoryRepository.FindByNameAsync(name.FirstCharToUpperOnly());
        }
    }
}
