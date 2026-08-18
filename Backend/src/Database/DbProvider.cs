using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;
using System;

namespace DoughBro.src.Database
{
    public interface IDbProvider
    {
        FirestoreDb GetFirestoreDb();
    }

    public class DbProvider : IDbProvider
    {
        private readonly FirestoreDb _db;

        public DbProvider(IConfiguration config)
        {
            string projectId = config["Firebase:project_id"]
                ?? throw new InvalidOperationException("Missing 'Firebase:project_id' from configuration.");

            _db = new FirestoreDbBuilder
            {
                ProjectId = projectId
            }.Build();
        }

        public FirestoreDb GetFirestoreDb() => _db;
    }
}
