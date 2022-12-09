namespace MoneyPlus.Data.Entities;

public class Wallet
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public int Balance { get; set; }

    public string UserId { get; set; }
    public IdentityUser User { get; set; }
}