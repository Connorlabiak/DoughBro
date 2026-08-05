using System.Text.Json;

namespace DoughBro.src.Services.Interfaces
{
    public interface IPlaidService
    {
        /// <summary>
        /// Creates a Plaid Link token for the authenticated user.
        /// </summary>
        /// <param name="userId">The authenticated Firebase user ID.</param>
        /// <returns>A Plaid Link token.</returns>
        Task<string> CreateLinkTokenAsync(string userId);

        /// <summary>
        /// Exchanges a Plaid public token for an access token and stores it for the user.
        /// </summary>
        /// <param name="publicToken">The Plaid public token returned by Link.</param>
        /// <param name="userId">The authenticated Firebase user ID.</param>
        /// <param name="institutionName">The linked institution display name.</param>
        /// <returns>The Plaid item ID.</returns>
        Task<string> ExchangePublicTokenAsync(string publicToken, string userId, string institutionName);

        /// <summary>
        /// Fetches transaction changes from Plaid's sync endpoint.
        /// </summary>
        /// <param name="accessToken">The stored Plaid access token.</param>
        /// <param name="cursor">The optional Plaid sync cursor.</param>
        /// <returns>The raw Plaid transaction sync payload.</returns>
        Task<JsonElement?> FetchTransactionsAsync(string accessToken, string? cursor = null);
    }
}
