using DoughBro.src.DTOs;
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

        public TransactionService(IDbProvider dbProvider, ITransactionRepository transactionRepository, ICategoryService categoryService, IPlaidService plaidService)
        {
            _db = dbProvider.GetFirestoreDb();
            _transactionRepository = transactionRepository;
            _categoryService = categoryService;
            _plaidService = plaidService;
        }

        public async Task ImportFromPlaid(string userId)
        {
            JsonElement transactions = await _plaidService.FetchTransactionsAsync(userId);
            List<TransactionModel> transactionModels = new List<TransactionModel>();
            foreach (JsonElement item in transactions.GetProperty("added").EnumerateArray())
            {
                string txId = item.GetProperty("transaction_id").GetString()!;
                decimal amount = item.GetProperty("amount").GetDecimal();
                string date = item.GetProperty("date").GetString()!;
                string name = item.GetProperty("name").GetString()!;
                string category = "unsorted";
                bool isPending = item.GetProperty("pending").GetBoolean();
                transactionModels.Add(new TransactionModel
                {
                    PlaidTransactionId = txId,
                    UserId = userId,
                    Name = name,
                    Date = date,
                    Amount = amount,
                    IsPending = isPending,
                });
            }
            if (transactionModels.Count > 0)
            {
                await _transactionRepository.SaveBatch(transactionModels, userId);
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
