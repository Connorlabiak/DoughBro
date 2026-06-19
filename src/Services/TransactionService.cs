using Google.Cloud.Firestore;
using DoughBro.src.DTOs;
using DoughBro.src.Models;
using DoughBro.src.Repositories.Interfaces;
using DoughBro.src.Services.Interfaces;

namespace DoughBro.src.Services
{
    public class TransactionService: ITransactionService
    {
        private readonly FirestoreDb _db;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(IDbProvider dbProvider, ITransactionRepository transactionRepository)
        {
            _db = dbProvider.GetFirestoreDb();
            _transactionRepository = transactionRepository;
        }

        public async Task<TransactionModel> CreateAsync(TransactionDto transactionDto)
        {
            TransactionModel model = new TransactionModel
            {
                UserId = transactionDto.UserId,
                Name = transactionDto.Name,
                Date = transactionDto.Date,
                Amount = transactionDto.Amount,
                Description = transactionDto.Description,
                category = transactionDto.category
            };
            await _transactionRepository.CreateAsync(model);
            return model;
            
        }

        public async Task<TransactionDto?> GetAsync(string id)
        {
            TransactionModel? model = await _transactionRepository.GetAsync(id);
            if (model == null)
            {
                return null;
            }

            return new TransactionDto
            {
                UserId = model.UserId,
                Name = model.Name,
                Date = model.Date,
                Amount = model.Amount,
                Description = model.Description,
                category = model.category
            };
        }
    }
}
