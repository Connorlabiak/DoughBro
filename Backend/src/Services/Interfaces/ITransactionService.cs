using DoughBro.src.DTOs;

namespace DoughBro.src.Services.Interfaces
{
    public interface ITransactionService
    {
        /// <summary>
        /// Synchronizes all linked Plaid accounts for the authenticated user.
        /// </summary>
        /// <param name="userId">The authenticated Firebase user ID.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SyncAllUserAccounts(string userId);

        /// <summary>
        /// Gets transactions for the authenticated user.
        /// </summary>
        /// <param name="userId">The authenticated Firebase user ID.</param>
        /// <param name="limit">The maximum number of transactions to return.</param>
        /// <returns>The user's transactions as DTOs.</returns>
        Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync(string userId, int limit);

        /// <summary>
        /// Updates the category assigned to a transaction.
        /// </summary>
        /// <param name="userId">The authenticated Firebase user ID.</param>
        /// <param name="transactionId">The transaction document ID.</param>
        /// <param name="category">The category to assign.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateTransactionCategoryAsync(string userId, string transactionId, string category);
    }
}
