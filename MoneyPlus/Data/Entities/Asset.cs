namespace MoneyPlus.Data.Entities;

public class Asset
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; }
}
