namespace MoneyPlus.Pages.Reports;

public class AgregatedSalesByCategory
{
    public int TotalAmount { get; set; }
    public string Category { get; set; }
}

public class AgregatedSalesByPayee
{
    public int TotalAmount { get; set; }
    public string Payee { get; set; }
}

public class AgregatedSalesByAsset
{
    public int TotalAmount { get; set; }
    public string Asset { get; set; }
}

[Authorize]
public class MonthlyExpensesModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public MonthlyExpensesModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Sales> Sales { get; set; } = default!;

    public IList<AgregatedSalesByCategory> AgregatedSalesByCategory { get; set; }

    public IList<AgregatedSalesByPayee> AgregatedSalesByPayee { get; set; }

    public IList<AgregatedSalesByAsset> AgregatedSalesByAsset { get; set; }

    public async Task OnGetAsync(int? Month, int? Year)
    {
        int month = Month.HasValue ? Month.Value : DateTime.Now.Month;
        int year = Year.HasValue ? Year.Value : DateTime.Now.Year;

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (_context.Sales != null)
        {
            Sales = await _context.Sales.Where(p => p.UserId == userId && p.Date.Month == month && p.Date.Year == year)
            .Include(s => s.CategoryType)
            .Include(s => s.Payee)
            .Include(s => s.Transaction)
            .Include(s => s.User).ToListAsync();
        }

        if (Sales == null || Sales.Count == 0)
        {
            Sales = new List<Sales>();
            Sales.Add(
                new Sales()
                {
                    Date = new DateTime(year, month, 1),
                    Amount = 0,
                    Description = "---- NO SALES ----",
                    CategoryType = new CategoryType() { Name = "" },
                    Payee = new Payee() { Name = "" }
                }
                );
        }

        AgregatedSalesByCategory =
            Sales.GroupBy(c => c.CategoryType.Name).
            Select(a => new AgregatedSalesByCategory() { Category = a.Key, TotalAmount = a.Sum(p => p.Amount) }).ToList();

        AgregatedSalesByPayee =
            Sales.GroupBy(p => p.Payee.Name).
            Select(a => new Reports.AgregatedSalesByPayee() { Payee = a.Key, TotalAmount = a.Sum(t => t.Amount) }).ToList();

        //TODO
        //AgregatedSalesByAsset =
        //    Sales.GroupBy(p => p.Asset).
        //    Select(a => new Reports.AgregatedSalesByPayee() { Payee = a.Key, TotalAmount = a.Sum(t => t.Amount) }).ToList();
    }
}
