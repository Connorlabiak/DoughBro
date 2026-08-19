using DoughBro.src.DTOs;
using DoughBro.src.Exceptions;
using DoughBro.src.Models;
using DoughBro.src.Repositories.Interfaces;
using DoughBro.src.Services.Interfaces;
using System.Text.Json;

namespace DoughBro.src.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IPlaidService _plaidService;
        private readonly IUserService _userService;

        public TransactionService(ITransactionRepository transactionRepository, IPlaidService plaidService, IUserService userService)
        {
            _transactionRepository = transactionRepository;
            _plaidService = plaidService;
            _userService = userService;
        }

        private async Task ImportFromSinglePlaidLink(PlaidAccessTokenModel token)
        {
            bool hasMore = true;
            string currentCursor = token.NextCursor ?? string.Empty;
            int retryCount = 0;
            const int maxMutationRetries = 3;

            while (hasMore)
            {
                try
                {
                    JsonElement? incomingTransactionsOrNull = await _plaidService.FetchTransactionsAsync(token.Token, currentCursor);
                    if (incomingTransactionsOrNull is null)
                    {
                        return;
                    }

                    JsonElement incomingTransactions = incomingTransactionsOrNull.Value;
                    currentCursor = incomingTransactions.GetProperty("next_cursor").GetString()!;
                    hasMore = incomingTransactions.GetProperty("has_more").GetBoolean();

                    List<string> idsToRemove = new List<string>();
                    foreach (JsonElement removed in incomingTransactions.GetProperty("removed").EnumerateArray())
                    {
                        idsToRemove.Add(removed.GetProperty("transaction_id").GetString()!);
                    }

                    await _transactionRepository.DeleteBatch(idsToRemove, token.UserId);

                    List<TransactionModel> transactionsToUpsert = new List<TransactionModel>();
                    var addedTransactions = incomingTransactions.GetProperty("added").EnumerateArray();
                    var modifiedTransactions = incomingTransactions.GetProperty("modified").EnumerateArray();
                    foreach (JsonElement item in addedTransactions.Concat(modifiedTransactions))
                    {
                        transactionsToUpsert.Add(new TransactionModel
                        {
                            Id = item.GetProperty("transaction_id").GetString()!,
                            Origin = "plaid",
                            UserId = token.UserId,
                            Name = item.GetProperty("name").GetString()!,
                            Date = item.GetProperty("date").GetString()!,
                            Amount = item.GetProperty("amount").GetDecimal(),
                            IsPending = item.GetProperty("pending").GetBoolean(),
                            MerchantName = item.GetProperty("merchant_name").GetString(),
                            Category = "unsorted"
                        });
                    }

                    await _transactionRepository.SaveBatch(transactionsToUpsert, token.UserId);
                }
                catch (PlaidApiException ex) when (ex.ErrorCode == "TRANSACTIONS_SYNC_MUTATION_DURING_PAGINATION")
                {
                    retryCount++;
                    if (retryCount > maxMutationRetries)
                    {
                        throw new Exception("Plaid synchronization failed after multiple retries due to mutations during pagination.");
                    }

                    currentCursor = token.NextCursor ?? string.Empty;
                    hasMore = true;
                    await Task.Delay(1000);
                    continue;
                }
            }

            token.NextCursor = currentCursor;
            await _userService.UpdatePlaidCursor(token, currentCursor);
        }

        public async Task SyncAllUserAccounts(string userId)
        {
            IEnumerable<PlaidAccessTokenModel>? tokens = await _userService.FetchPlaidAccessTokens(userId);
            if (tokens is null)
            {
                return;
            }

            foreach (PlaidAccessTokenModel token in tokens)
            {
                await ImportFromSinglePlaidLink(token);
            }
        }

        public async Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync(string userId, int limit)
        {
            IEnumerable<TransactionModel> transactions = await _transactionRepository.GetAllTransactions(userId, limit);
            return transactions.Select(transaction => new TransactionDto
            {
                Id = transaction.Id,
                Origin = transaction.Origin,
                UserId = transaction.UserId,
                Name = transaction.Name,
                Date = transaction.Date,
                Amount = transaction.Amount,
                Description = transaction.Description,
                IsPending = transaction.IsPending,
                MerchantName = transaction.MerchantName,
                Category = transaction.Category,
            });
        }

        public async Task UpdateTransactionCategoryAsync(string userId, string transactionId, string category)
        {
            await _transactionRepository.UpdateCategoryAsync(userId, transactionId, category);
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionsByCategoryAsync(string userId, string categoryId)
        {
            IEnumerable<TransactionModel> transactions = await _transactionRepository.GetTransactionsByCategoryAsync(userId, categoryId);
            return transactions.Select(transaction => new TransactionDto
            {
                Id = transaction.Id,
                Origin = transaction.Origin,
                UserId = transaction.UserId,
                Name = transaction.Name,
                Date = transaction.Date,
                Amount = transaction.Amount,
                Description = transaction.Description,
                IsPending = transaction.IsPending,
                MerchantName = transaction.MerchantName,
                Category = transaction.Category,
            });
        }

        public async Task ClearCategoryAsync(string userId, string categoryId)
        {
            await _transactionRepository.ClearCategoryAsync(userId, categoryId);
        }
    }
}
