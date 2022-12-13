namespace MoneyPlus.Pages.Reports;

public class TotalExpensesByYearModel : PageModel
{
    private readonly MoneyPlus.Data.ApplicationDbContext _context;

    public TotalExpensesByYearModel(MoneyPlus.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Expense> Expense { get; set; } = default!;

    public async Task OnGetAsync()
    {
        if (_context.Expenses != null)
        {
            Expense = await _context.Expenses
            .Include(e => e.Asset)
            .Include(e => e.CategoryType)
            .Include(e => e.Payee)
            .Include(e => e.Transaction)
            .Include(e => e.User).ToListAsync();
        }
    }
}
