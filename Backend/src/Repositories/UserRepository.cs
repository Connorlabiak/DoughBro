using DoughBro.src.Models;
using DoughBro.src.Repositories.Interfaces;
using Google.Cloud.Firestore;
using Newtonsoft.Json.Linq;
using static Google.Rpc.Context.AttributeContext.Types;

namespace DoughBro.src.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly FirestoreDb _db;

        public UserRepository(IDbProvider dbProvider)
        {
            _db = dbProvider.GetFirestoreDb();
        }

        public async Task SavePlaidAccessToken(PlaidAccessTokenModel token)
        {
            var itemRef = _db.Collection("users").Document(token.UserId).Collection("plaid_items").Document(token.ItemId);
            await itemRef.SetAsync(token);
        }

        public async Task<PlaidAccessTokenModel?> FetchPlaidAccessToken(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
