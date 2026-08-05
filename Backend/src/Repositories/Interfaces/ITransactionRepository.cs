using DoughBro.src.Models;

namespace DoughBro.src.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task SaveBatch(IEnumerable<TransactionModel> transactions, string userId);

        Task DeleteBatch(IEnumerable<string> transactionIds, string userId);

        Task<IEnumerable<TransactionModel>> GetAllTransactions(string userId, int limit);
    }
}
