using DoughBro.src.Models;

namespace DoughBro.src.Repositories.Interfaces
{
    public interface IUserRepository
    {
        /// <summary>
        /// Saves the Plaid access token for a user to Firestore.
        /// </summary>
        /// <param name="token"> The access token.</param>
        /// <returns></returns>
        Task SavePlaidAccessToken(PlaidAccessTokenModel token);

        /// <summary>
        /// Fetches the Plaid access token for a user from Firestore.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<PlaidAccessTokenModel>?> FetchPlaidAccessTokens(string userId);

        /// <summary>
        /// Updates the Plaid cursor for a given access token in Firestore.
        /// </summary>
        /// <param name="token">The targeted token</param>
        /// <param name="currentCursor">The most recent cursor</param>
        /// <returns></returns>
        public Task UpdatePlaidCursor(PlaidAccessTokenModel token, string currentCursor);
    }
}
