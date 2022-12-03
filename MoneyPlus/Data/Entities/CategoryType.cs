namespace MoneyPlus.Data.Entities;

public class CategoryType
{
    public int Id { get; set; }
    [MaxLength(50)]
    public string Name { get; set; }
    public string CategoryId { get; set; }
    public Category Categories { get; set; }
}