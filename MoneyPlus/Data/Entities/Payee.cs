namespace MoneyPlus.Data.Entities;

public class Payee
{
    public int Id { get; set; }
    [MaxLength(50)]
    public string Name { get; set; }
    public int NIF { get; set; }
}