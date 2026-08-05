using DoughBro.src.Models;

namespace DoughBro.src.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        /// <summary>
        /// Saves transaction models to Firestore in batches.
        /// </summary>
        /// <param name="transactions">The transactions to save.</param>
        /// <param name="userId">The authenticated Firebase user ID.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SaveBatch(IEnumerable<TransactionModel> transactions, string userId);

        /// <summary>
        /// Deletes transaction documents from Firestore in batches.
        /// </summary>
        /// <param name="transactionIds">The Plaid transaction IDs to delete.</param>
        /// <param name="userId">The authenticated Firebase user ID.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteBatch(IEnumerable<string> transactionIds, string userId);

        /// <summary>
        /// Gets transactions for a user from Firestore.
        /// </summary>
        /// <param name="userId">The authenticated Firebase user ID.</param>
        /// <param name="limit">The maximum number of transactions to return.</param>
        /// <returns>The user's transaction models.</returns>
        Task<IEnumerable<TransactionModel>> GetAllTransactions(string userId, int limit);
    }
}
