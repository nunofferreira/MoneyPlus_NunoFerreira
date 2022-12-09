namespace MoneyPlus.Data.Entities;

public class Expense
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    [Column(TypeName = "decimal(18,2)")]
    public int Amount { get; set; }
    public string PaymentMethod { get; set; }
    [MaxLength(1000)]
    public string Description { get; set; }

    public string UserId { get; set; }
    public IdentityUser User { get; set; }
    public int PayeeId { get; set; }
    public Payee Payee { get; set; }
    public int? TransactionId { get; set; }
    public Transaction Transaction { get; set; }
    public int CategoryTypeId { get; set; }
    public CategoryType CategoryType { get; set; }
    public int? AssetId { get; set; }
    public Asset Asset { get; set; }
    
    [NotMapped]
    public int WalletId { get; set; }
}