using DoughBro.src.Models;

namespace DoughBro.src.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        public Task SaveBatch(IEnumerable<TransactionModel> transactions, string userId);

        public Task DeleteBatch(IEnumerable<string> transactionIds, string userId);
    }
}
