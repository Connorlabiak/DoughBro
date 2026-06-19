using DoughBro.src.Models;

namespace DoughBro.src.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task CreateAsync(TransactionModel transactionModel);
        Task<TransactionModel?> GetAsync(string id);
    }
}
