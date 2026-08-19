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
        /// Updates the editable fields and category assigned to a transaction.
        /// </summary>
        /// <param name="userId">The authenticated Firebase user ID.</param>
        /// <param name="transactionId">The transaction document ID.</param>
        /// <param name="request">The transaction fields to persist.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateTransactionAsync(string userId, string transactionId, UpdateTransactionCategoryRequest request);

        /// <summary>
        /// Gets all transactions assigned to a category.
        /// </summary>
        /// <param name="userId">The authenticated Firebase user ID.</param>
        /// <param name="categoryId">The category document ID.</param>
        /// <returns>The category's transactions as DTOs.</returns>
        Task<IEnumerable<TransactionDto>> GetTransactionsByCategoryAsync(string userId, string categoryId);

        /// <summary>
        /// Removes a category assignment from every matching transaction.
        /// </summary>
        /// <param name="userId">The authenticated Firebase user ID.</param>
        /// <param name="categoryId">The category document ID.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ClearCategoryAsync(string userId, string categoryId);
    }
}
