namespace MoneyPlus.Pages.Reports;

public class YearlyByCategory
{
    public List<SubCategoryValue> SubCategories { get; set; }
}
public class SubCategoryValue
{
    public string SubCategoryName { get; set; }
    public string CategoryName { get; set; }
    public int Amount { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
}



[Authorize]
public class YearlyExpensesModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public YearlyExpensesModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public YearlyByCategory ExpensesBySubCategory { get; set; } = default!;

    public async Task OnGetAsync(int? Year)
    {
        int year = Year.HasValue ? Year.Value : DateTime.Now.Year;

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        List<Expense> Expenses = null;

        if (_context.Expenses != null)
        {
            Expenses = await _context.Expenses.Where(p => p.UserId == userId && p.Date.Year == year)
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
                    Date = new DateTime(year, 1, 1),
                    Amount = 0,
                    Description = "---- NO SALES ----",
                    CategoryType = new CategoryType() { Name = "" },
                    Payee = new Payee() { Name = "" },
                    Asset = new Asset() { Name  =""}
                }
                );
        }

        //Transform Expenses into YearlyByCategory
        var result = Expenses.GroupBy(p => new { p.Date.Month, subCategory = p.CategoryType.Name, p.CategoryType.Category.Name })
            .Select(s => new SubCategoryValue()
            { Year = year,
                Month = s.Key.Month,
                CategoryName = s.Key.Name,
                SubCategoryName = s.Key.subCategory,
                Amount = s.Sum(x => x.Amount) }).ToList();

        ExpensesBySubCategory = new YearlyByCategory();
        ExpensesBySubCategory.SubCategories = result;

    }
}
