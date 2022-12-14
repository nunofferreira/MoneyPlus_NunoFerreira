namespace MoneyPlus.Pages.Reports;

public class TotalExpensesByYear
{
    public List<CategoryValueByYear> Categories { get; set; }
}

public class CategoryValueByYear
{
    public List<SubCategoryValueByYear> SubCategoryList { get; set; }
    public string CategoryName { get; set; }
    public List<MonthlyValuesByYear> MonthlyValuesByYear { get; set; }
}

public class SubCategoryValueByYear
{
    public string SubCategoryName { get; set; }
    public List<MonthlyValuesByYear> MonthlyValuesByYear { get; set; }
}

public class MonthlyValuesByYear
{
    public int Amount { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
}
public class CategoryFlatValueByYear
{
    public string SubCategoryName { get; set; }
    public string CategoryName { get; set; }
    public int Amount { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
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

    public async Task OnGetAsync(int? Year)
    {
        int year = Year ?? DateTime.Now.Year;

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        List<Expense> Expenses = null;

        if (_context.Expenses != null)
        {
            Expenses = await _context.Expenses.Where(p => p.UserId == userId && p.Date.Year == year)
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

        var result = Expenses.GroupBy(p => new { p.Date.Month, subCategory = p.CategoryType?.Name ?? "- No Sub Category -", category = p.CategoryType?.Category?.Name ?? "- No Categories -" })
            .Select(s => new CategoryFlatValueByYear()
            {
                Year = year,
                Month = s.Key.Month,
                CategoryName = s.Key.category,
                SubCategoryName = s.Key.subCategory,
                Amount = s.Sum(x => x.Amount)
            }).OrderBy(p => p.CategoryName).ToList();

        YearlyExpensesBySubCategory = new TotalExpensesByYear();
        YearlyExpensesBySubCategory.Categories = new List<CategoryValueByYear>();

        var categoryValuesListByYear = result.GroupBy(c => new { c.CategoryName }).Select(p =>
        new CategoryValueByYear() { CategoryName = p.Key.CategoryName, MonthlyValuesByYear = new List<MonthlyValuesByYear>() }).ToList();

        foreach (var category in categoryValuesListByYear)
        {
            category.MonthlyValuesByYear.AddRange
                (result.Where(p => p.CategoryName.Equals(category.CategoryName)).GroupBy(p => new { p.Month, p.Year })
                .Select(a => new MonthlyValuesByYear()
                {
                    Month = a.Key.Month,
                    Year = a.Key.Year,
                    Amount = a.Sum(x => x.Amount)
                }).ToList());

        }
        foreach (var newCategory in categoryValuesListByYear)
        {
            newCategory.SubCategoryList = new List<SubCategoryValueByYear>();

            foreach (var subCategories in result.Where(p => p.CategoryName.Equals(newCategory.CategoryName)))
            {
                var subCategory = newCategory.SubCategoryList.FirstOrDefault(p => p.SubCategoryName.Equals(subCategories.CategoryName));

                if (subCategory == null)
                {
                    subCategory = new SubCategoryValueByYear()
                    {
                        SubCategoryName = subCategories.SubCategoryName
                    };
                    newCategory.SubCategoryList.Add(subCategory);
                }

                if (subCategory.MonthlyValuesByYear == null)
                {
                    subCategory.MonthlyValuesByYear = new List<MonthlyValuesByYear>();
                }

                subCategory.MonthlyValuesByYear.Add(new MonthlyValuesByYear()
                {
                    Amount = subCategories.Amount,
                    Month = subCategories.Month,
                    Year = subCategories.Year,
                });
            }

            YearlyExpensesBySubCategory.Categories = categoryValuesListByYear;
        }
    }
}
