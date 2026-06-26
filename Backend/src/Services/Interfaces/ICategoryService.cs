using DoughBro.src.DTOs;
using DoughBro.src.Models;

namespace DoughBro.src.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryModel> CreateAsync(CategoryDto catgoryDto);
        Task<CategoryModel?> FindAsync(string name);
    }
}
