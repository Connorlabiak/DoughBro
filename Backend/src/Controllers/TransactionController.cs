using Microsoft.AspNetCore.Mvc;
using DoughBro.src.Services.Interfaces;
using DoughBro.src.DTOs;
using Microsoft.AspNetCore.Authorization;
using DoughBro.src.Extensions;

namespace DoughBro.src.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("sync")]
        public async Task<ActionResult> SyncTransactionsAsync()
        {
            string? userId = User.GetUserId();
            if (userId is null)
            {
                return Unauthorized("User ID not found in claims.");
            }
            await _transactionService.SyncAllUserAccounts(userId);
            return Ok(new { message = "Transactions synced successfully" });
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetTransactions([FromQuery] int limit = 50)
        {
            string? userId = User.GetUserId();
            if (userId is null)
            {
                return Unauthorized("User ID not found in claims");
            }
            IEnumerable<TransactionDto> transactions = await _transactionService.GetAllTransactionsAsync(userId, limit);
            if (transactions is null)
            {
                return StatusCode(500, new { message = "Failed to fetch transactions" });
            }
            else
            {
                return Ok(transactions);
            }

        }

        [HttpPatch("{transactionId}")]
        public async Task<IActionResult> UpdateTransaction(string transactionId, [FromBody] UpdateTransactionCategoryRequest request)
        {
            string? userId = User.GetUserId();
            if (userId is null)
            {
                return Unauthorized("User ID not found in claims");
            }

            if (string.IsNullOrWhiteSpace(transactionId) || string.IsNullOrWhiteSpace(request.Category) || !DateOnly.TryParse(request.Date, out _))
            {
                return BadRequest(new { message = "A transaction ID, name, valid date, and category are required" });
            }

            await _transactionService.UpdateTransactionAsync(userId, transactionId, request);
            return NoContent();
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetTransactionsByCategory(string categoryId)
        {
            string? userId = User.GetUserId();
            if (userId is null)
            {
                return Unauthorized("User ID not found in claims");
            }

            IEnumerable<TransactionDto> transactions = await _transactionService.GetTransactionsByCategoryAsync(userId, categoryId);
            return Ok(transactions);
        }

    }
}
