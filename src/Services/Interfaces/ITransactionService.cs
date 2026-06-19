using DoughBro.src.DTOs;
using DoughBro.src.Models;

namespace DoughBro.src.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionModel> CreateAsync(TransactionDto transactionDto);

        Task<TransactionDto?> GetAsync(string id);
    }
}
