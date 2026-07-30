using DoughBro.src.DTOs;
using DoughBro.src.Models;
using System.Text.Json;

namespace DoughBro.src.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionModel> CreateAsync(TransactionDto transactionDto);

        Task<TransactionDto?> GetAsync(string id);

        Task SaveBatch(JsonElement transactions, string userId, string transactionId);
    }
}
