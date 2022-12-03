namespace MoneyPlus.Data.Entities;

public class CategoryType
{
    public int Id { get; set; }
    [MaxLength(50)]
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}