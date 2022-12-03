namespace MoneyPlus.Data.Entities;

public class Transaction
{
    public int Id { get; set; }
    public int Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string Type { get; set; } // Credit or Debit
    public string TransctionType { get; set; } // Money, BitCoin, etc
    [MaxLength(100)]
    public int WalletId { get; set; }
    public Wallet Wallet { get; set; }
}