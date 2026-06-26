using DoughBro.src.DTOs;
using DoughBro.src.Models;
using DoughBro.src.Repositories.Interfaces;
using DoughBro.src.Services.Interfaces;
using Google.Cloud.Firestore;
using Grpc.Core;

namespace DoughBro.src.Services
{
    public class TransactionService: ITransactionService
    {
        private readonly FirestoreDb _db;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICategoryService _categoryService;

        public TransactionService(IDbProvider dbProvider, ITransactionRepository transactionRepository, ICategoryService categoryService)
        {
            _db = dbProvider.GetFirestoreDb();
            _transactionRepository = transactionRepository;
            _categoryService = categoryService;
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
            string id = await _transactionRepository.CreateAsync(model);
            model.Id = id;
            return model;
            
        }

        public async Task updateTransactionCategory(string transactionId, string category)
        {
            CategoryModel? categoryModel = await _categoryService.FindAsync(category);
            if (categoryModel is null)
            {
                throw new MissingMemberException($"Category: {category} does not exist.");
            }
            try
            {
                await _transactionRepository.UpdateCategoryAsync(transactionId, categoryModel);
            } catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
            {
                throw new MissingMemberException($"Transaction with Id: {transactionId} not found.");
            }

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
