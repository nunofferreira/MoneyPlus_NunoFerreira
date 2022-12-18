namespace MoneyPlus.Pages.Reports;

public class AgregatedExpensesByCategory
{
    public int TotalAmount { get; set; }
    public string Category { get; set; }
}

public class AgregatedExpensesByPayee
{
    public int TotalAmount { get; set; }
    public string Payee { get; set; }
}

public class AgregatedExpensesByAsset
{
    public int TotalAmount { get; set; }
    public string Asset { get; set; }
}

[Authorize]
public class MonthlyExpensesModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<IndexModel> _logger;

    public MonthlyExpensesModel(ApplicationDbContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IList<Expense> Expenses { get; set; } = default!;

    public IList<AgregatedExpensesByCategory> AgregatedExpensesByCategory { get; set; }

    public IList<AgregatedExpensesByPayee> AgregatedExpensesByPayee { get; set; }

    public IList<AgregatedExpensesByAsset> AgregatedExpensesByAsset { get; set; }

    public async Task OnGetAsync(int? Month, int? Year)
    {
        int month = Month.HasValue ? Month.Value : DateTime.Now.Month;
        int year = Year.HasValue ? Year.Value : DateTime.Now.Year;

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (_context.Expenses != null)
        {
            Expenses = await _context.Expenses.Where(p => p.UserId == userId && p.Date.Month == month && p.Date.Year == year)
            .Include(s => s.CategoryType)
            .Include(s => s.Payee)
            .Include(s => s.Transaction)
            .Include(s => s.Asset)
            .Include(s => s.User).ToListAsync();
        }

        if (Expenses == null || Expenses.Count == 0)
        {
            Expenses = new List<Expense>();
            Expenses.Add(
                new Expense()
                {
                    Date = new DateTime(year, month, 1),
                    Amount = 0,
                    Description = "---- NO EXPENSES ----",
                    CategoryType = new CategoryType() { Name = "" },
                    Payee = new Payee() { Name = "" },
                    Asset = new Asset() { Name = "" }
                });
        }

        AgregatedExpensesByCategory =
            Expenses.GroupBy(c => c.CategoryType.Name).
            Select(a => new AgregatedExpensesByCategory() { Category = a.Key, TotalAmount = a.Sum(p => p.Amount) }).ToList();

        AgregatedExpensesByPayee =
            Expenses.GroupBy(p => p.Payee.Name).
            Select(a => new Reports.AgregatedExpensesByPayee() { Payee = a.Key, TotalAmount = a.Sum(t => t.Amount) }).ToList();

        AgregatedExpensesByAsset =
            Expenses.GroupBy(p => p.Asset?.Name ?? " - No Asset - ").
            Select(a => new Reports.AgregatedExpensesByAsset() { Asset = a.Key, TotalAmount = a.Sum(t => t.Amount) }).ToList();
    }
}