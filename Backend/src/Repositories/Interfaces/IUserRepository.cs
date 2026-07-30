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
        Task<PlaidAccessTokenModel?> FetchPlaidAccessToken(string userId);
    }
}
