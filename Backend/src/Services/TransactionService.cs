using DoughBro.src.Database;
using DoughBro.src.DTOs;
using DoughBro.src.Exceptions;
using DoughBro.src.Models;
using DoughBro.src.Repositories.Interfaces;
using DoughBro.src.Services.Interfaces;
using Google.Cloud.Firestore;
using Grpc.Core;
using System.Text.Json;

namespace DoughBro.src.Services
{
    public class TransactionService: ITransactionService
    {
        private readonly FirestoreDb _db;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICategoryService _categoryService;
        private readonly IPlaidService _plaidService;
        private readonly IUserService _userService;

        public TransactionService(IDbProvider dbProvider, ITransactionRepository transactionRepository, ICategoryService categoryService, IPlaidService plaidService, IUserService userService)
        {
            _db = dbProvider.GetFirestoreDb();
            _transactionRepository = transactionRepository;
            _categoryService = categoryService;
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
                    JsonElement incomingTransactions = incomingTransactionsOrNull!.Value;
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

        public async Task<TransactionModel> CreateAsync(TransactionDto transactionDto)
        {
           throw new NotImplementedException();
        }

        public async Task updateTransactionCategory(string transactionId, string category)
        {
            throw new NotImplementedException();
        }

        public async Task<TransactionDto?> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task SaveBatch(JsonElement transactions, string userId, string transactionId)
        {
            throw new NotImplementedException();
        }
    }
}
