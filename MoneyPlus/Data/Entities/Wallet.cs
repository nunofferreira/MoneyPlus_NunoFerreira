namespace MoneyPlus.Data.Entities;

public class Wallet
{
    public int Id { get; set; }
    public int Balance { get; set; }
    public string UserId { get; set; }
    public IdentityUser User { get; set; }
}