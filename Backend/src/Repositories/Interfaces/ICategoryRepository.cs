using DoughBro.src.Models;

namespace DoughBro.src.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<string> CreateAsync(CategoryModel categoryModel);

        public Task<CategoryModel?> FindByNameAsync(string name);

    }
}
