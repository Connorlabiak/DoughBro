namespace DoughBro.src.Models
{
    public class TransactionDbModel()
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public double Amount { get; set; }
        public string? Description { get; set; }
        public string? category { get; set; }
    }
}
