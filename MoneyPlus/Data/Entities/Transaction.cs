namespace MoneyPlus.Data.Entities;

public enum TransactionType
{
    Credit,
    Debit
}
public class Transaction
{
    public int Id { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public int Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// Credit or Debit
    /// </summary>
    public TransactionType Type { get; set; }
    /// <summary>
    /// Money, BitCoin, etc
    /// </summary>
    public string TransactionType { get; set; }

    public int WalletId { get; set; }
    public Wallet Wallet { get; set; }
}