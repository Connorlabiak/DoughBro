using Google.Cloud.Firestore;

namespace DoughBro.src.Database
{
    public class FirestoreDecimalConverter : IFirestoreConverter<decimal>
    {
        public object ToFirestore(decimal value) => (double)value;

        public decimal FromFirestore(object value)
        {
            if (value is double d) return (decimal)d;
            if (value is long l) return (decimal)l;

            throw new ArgumentException($"Cannot convert object type {value.GetType()} to decimal.");
        }
    }
}
