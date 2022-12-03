namespace MoneyPlus.Data.Entities;

public class Sales
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public int Amount { get; set; }
    public string PaymentMethod { get; set; }
    public string Description { get; set; }
    public string? Asset { get; set; }
    public int UserId { get; set; }
    public IdentityUser User { get; set; }
    public int PayeeId { get; set; }
    public Payee Payees { get; set; }
    public int? TransactionId { get; set; }
    public Transaction Transactions { get; set; }
    public int CategoryTypeId { get; set; }
    public CategoryType CategoryTipes { get; set; }
}