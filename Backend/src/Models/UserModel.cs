using Google.Cloud.Firestore;

namespace DoughBro.src.Models
{
	[FirestoreData]
	public class UserModel
	{
		[FirestoreDocumentId]
		public required string Id { get; set; }

        [FirestoreProperty]
		public required string FirstName { get; set; }

        [FirestoreProperty]
		public required string LastName { get; set; }

		[FirestoreProperty]
		public DateOnly? Birthday { get; set; }
	}
}
