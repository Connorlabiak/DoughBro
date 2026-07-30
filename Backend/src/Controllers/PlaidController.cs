using DoughBro.src.Extensions;
using DoughBro.src.Services.Interfaces;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DoughBro.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PlaidController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly IPlaidService _plaidService;

    public PlaidController(ITransactionService transactionService, IPlaidService plaidService)
    {
        _transactionService = transactionService;
        _plaidService = plaidService;
    }

    [HttpPost("create-link-token")]
    public async Task<IActionResult> CreateLinkToken()
    {
        string? userId = User.GetUserId();
        if (userId is null)
        {
            return Unauthorized("User ID not found in claims.");
        }
        string token = await _plaidService.CreateLinkTokenAsync(userId);
        return Ok(new { linkToken = token});
    }

    [HttpPost("exchange-public-token")]
    public async Task<IActionResult> ExchangePublicToken([FromBody] ExchangeTokenRequest request)
    {
        string? userId = User.GetUserId();
        if (userId is null)
        {
            return Unauthorized("User ID not found in claims.");
        }
        var itemId = await _plaidService.ExchangePublicTokenAsync(request.PublicToken, userId, request.InstitutionName);

        // Sync initial transactions immediately
        //var syncResult = await _plaidService.FetchTransactionsAsync(accessToken);
        //await SaveTransactionsToFirestore(userId, itemId, syncResult);

        return Ok(new { success = true, itemId });
    }

}

public record ExchangeTokenRequest(string PublicToken, string InstitutionName);