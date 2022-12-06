namespace MoneyPlus.Data.Entities;

public class Transaction
{
    public int Id { get; set; }
    public int Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// Credit or Debit
    /// </summary>
    public string Type { get; set; }
    /// <summary>
    /// Money, BitCoin, etc
    /// </summary>
    public string TransactionType { get; set; }

    [MaxLength(100)]
    public int WalletId { get; set; }
    public Wallet Wallet { get; set; }
}