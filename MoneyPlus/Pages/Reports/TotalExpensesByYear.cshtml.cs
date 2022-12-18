namespace MoneyPlus.Pages.Reports;

public class TotalExpensesByYear
{
    public List<CategoryValueByYear> Categories { get; set; }
}

public class CategoryValueByYear
{
    public string CategoryName { get; set; }
    public List<YearlyValues> YearlyValues{ get; set; }
}

public class YearlyValues
{
    public int Amount { get; set; }
    public int Year { get; set; }
}
public class CategoryFlatValueByYear
{
    public string CategoryName { get; set; }
    public int Amount { get; set; }
    public int Year { get; set; }
}


[Authorize]
public class TotalExpensesByYearModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<IndexModel> _logger;

    public TotalExpensesByYearModel(ApplicationDbContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    [BindProperty]
    public TotalExpensesByYear YearlyExpensesBySubCategory { get; set; } = default!;
    public int? SelectedYear { get; set; }
    public async Task OnGetAsync(int? Year)
    {
        int year = Year ?? DateTime.Now.Year;
        SelectedYear = year;

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        List<Expense> Expenses = null;

        if (_context.Expenses != null)
        {
            Expenses = await _context.Expenses.Where(p => p.UserId == userId
            && (p.Date.Year >= year - 1 && p.Date.Year <= year + 1))
            .Include(s => s.CategoryType.Category)
            .Include(s => s.Payee)
            .Include(s => s.Transaction)
            .Include(s => s.Asset)
            .Include(s => s.CategoryType.Category)
            .Include(s => s.User).ToListAsync();
        }

        if (Expenses == null || Expenses.Count == 0)
        {
            Expenses = new List<Expense>();
            Expenses.Add(
                new Expense()
                {
                    Date = new DateTime(year, 1, 1),
                    Amount = 0,
                    Description = "---- NO SALES ----",
                    CategoryType = new CategoryType() { Name = "" },
                    Payee = new Payee() { Name = "" },
                    Asset = new Asset() { Name = "" }
                }
                );
        }

        var result = Expenses.GroupBy(p => new { p.Date.Year, category = p.CategoryType?.Category?.Name ?? "- No Categories -" })
            .Select(s => new CategoryFlatValueByYear()
            {
                Year = s.Key.Year,
                CategoryName = s.Key.category,
                Amount = s.Sum(x => x.Amount)
            }).OrderBy(p => p.CategoryName).ToList();

        YearlyExpensesBySubCategory = new TotalExpensesByYear();
        YearlyExpensesBySubCategory.Categories = new List<CategoryValueByYear>();

        var categoryValuesListByYear = result.GroupBy(c => new { c.CategoryName }).Select(p =>
        new CategoryValueByYear() { CategoryName = p.Key.CategoryName, YearlyValues = new List<YearlyValues>() }).ToList();

        foreach (var category in categoryValuesListByYear)
        {
            category.YearlyValues.AddRange
                (result.Where(p => p.CategoryName.Equals(category.CategoryName)).GroupBy(p => p.Year)
                .Select(a => new YearlyValues()
                {
                    Year = a.Key,
                    Amount = a.Sum(x => x.Amount)
                }).ToList());
        }
        YearlyExpensesBySubCategory.Categories = categoryValuesListByYear;

    }
}

