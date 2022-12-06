namespace MoneyPlus.Data.Repositories;

public class ExpensesRepository
{
    private readonly ApplicationDbContext _ctx;

    public ExpensesRepository(ApplicationDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<List<Transaction>> FindByFilterAsync(ExpensesFilter filter = null)
    {
        var expenses = _ctx.Transactions.AsQueryable();

        if (filter == null)
            return await expenses.ToListAsync();

        if (filter.Category != null)
            expenses = expenses.Where(exp => exp.TransactionType.Contains(filter.Category));

        if (filter.Payee != null)
            expenses = expenses.Where(exp => exp.TransactionType.Contains(filter.Payee));

        if (filter.Asset != null)
            expenses = expenses.Where(exp => exp.TransactionType.Contains(filter.Asset));

        return await expenses.ToListAsync();
    }
}
