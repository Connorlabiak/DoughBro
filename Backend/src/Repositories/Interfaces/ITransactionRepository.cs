using DoughBro.src.Models;

namespace DoughBro.src.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task<string> CreateAsync(TransactionModel transactionModel);

        Task UpdateCategoryAsync(string transactionId, CategoryModel categoryModel);

        Task<TransactionModel?> GetAsync(string id);
    }
}
