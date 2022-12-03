namespace MoneyPlus.Data.Entities;

public class Wallet
{
    public int Id { get; set; }
    public int Balance { get; set; }
    public int UserId { get; set; }
    public IdentityUser User { get; set; }
}