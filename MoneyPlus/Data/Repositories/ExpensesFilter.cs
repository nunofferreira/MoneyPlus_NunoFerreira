namespace MoneyPlus.Data.Repositories;

public class ExpensesFilter
{
    public string Category { get; set; }
    public string Payee { get; set; }
    public string Asset { get; set; }

    public ExpensesFilter()
    {
        Category = null;
        Payee = null;
        Asset = null;
    }
}