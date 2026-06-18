namespace DoughBro.Models
{
    public record class TransactionDbModel(string Id, string UserId, string Name, string Date, double Amount, string? Description, Category category)
    {
        string Id { get; set; }
        string UserId { get; set; }
        string Name { get; set; }
        string Date { get; set; }
        double Amount { get; set; }
        string? Description { get; set; }
        Category category { get; set; }
    }
}
