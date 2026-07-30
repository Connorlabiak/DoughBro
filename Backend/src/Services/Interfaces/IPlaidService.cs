using System.Text.Json;

namespace DoughBro.src.Services.Interfaces
{
    public interface IPlaidService
    {
        Task<string> CreateLinkTokenAsync(string userId);
        Task<string> ExchangePublicTokenAsync(string publicToken, string userId, string institutionName);
        Task<JsonElement?> FetchTransactionsAsync(string accessToken, string? cursor = null);
    }
}
