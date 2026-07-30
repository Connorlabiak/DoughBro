using Microsoft.AspNetCore.Mvc;
using DoughBro.src.Services;
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

        [HttpGet("sync")]
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


        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> CreateAsync([FromBody] TransactionDto transactionDto)
        {
            var result = await _transactionService.CreateAsync(transactionDto);
            if (result.Id == null)
            {
                return BadRequest("Failed to create transaction");
            }
            return Ok("TransactionCreated");
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<TransactionDto>> GetAsync(string id)
        {
            var result = await _transactionService.GetAsync(id);
            if (result == null)
            {
                return NotFound("Transaction not found");
            }
            return result;
        }
    }
}
