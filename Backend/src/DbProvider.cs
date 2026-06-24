using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using System.Text.Json;

namespace DoughBro.src
{
    public interface IDbProvider
    {
        FirestoreDb GetFirestoreDb();
    }

    public class DbProvider: IDbProvider
    {
        private readonly FirestoreDb _db;
        public DbProvider(IConfiguration config)
        {
            IConfigurationSection firebaseSection = config.GetSection("Firebase");
            if (!firebaseSection.Exists())
            {
                throw new InvalidOperationException("Firebase credential section is missing from secrets.json.");
            }

            string projectId = firebaseSection["project_id"] ?? throw new InvalidOperationException("Missing Firebase project_id from credential.");

            var configDictionary = firebaseSection.AsEnumerable().ToDictionary(k => k.Key.Replace("Firebase:", ""), v => v.Value);
            string serializedCredentials = JsonSerializer.Serialize(configDictionary);
            var serviceAccountCredential = CredentialFactory.FromJson<ServiceAccountCredential>(serializedCredentials);

            _db = new FirestoreDbBuilder
            {
                ProjectId = projectId,
                Credential = serviceAccountCredential.ToGoogleCredential()
            }.Build();
        }

        public FirestoreDb GetFirestoreDb()
        {
            return _db;
        }
    }
}
