using DoughBro.src.Database;
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

        public async Task<IEnumerable<PlaidAccessTokenModel>?> FetchPlaidAccessTokens(string userId)
        {
            QuerySnapshot tokens = await _db.Collection("users").Document(userId).Collection("plaid_items").GetSnapshotAsync();
            if (tokens.Count == 0)
            {
                return null;
            }
            IList<PlaidAccessTokenModel> tokenList = new List<PlaidAccessTokenModel>();
            foreach (DocumentSnapshot token in tokens.Documents)
            {
                tokenList.Add(new PlaidAccessTokenModel
                {
                    ItemId = token.Id,
                    Token = token.GetValue<string>("Token"),
                    UserId = token.GetValue<string>("UserId"),
                    InstitutionName = token.GetValue<string>("InstitutionName"),
                    CreatedAt = token.GetValue<string>("CreatedAt"),
                    NextCursor = token.GetValue<string?>("NextCursor")
                });
            }
            return tokenList;
        }

        public async Task SavePlaidAccessToken(PlaidAccessTokenModel token)
        {
            var itemRef = _db.Collection("users").Document(token.UserId).Collection("plaid_items").Document(token.ItemId);
            await itemRef.SetAsync(token);
        }

        public async Task UpdatePlaidCursor(PlaidAccessTokenModel token, string currentCursor)
        {
            var itemRef = _db.Collection("users").Document(token.UserId).Collection("plaid_items").Document(token.ItemId);
            await itemRef.UpdateAsync("NextCursor", currentCursor);
        }
    }
}
