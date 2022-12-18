namespace MoneyPlus.Pages.Reports;

public class YearlyByCategory
{
    public List<CategoryValue> Categories { get; set; }
}

public class CategoryValue
{
    public List<SubCategoryValue> SubCategoryList { get; set; }
    public string CategoryName { get; set; }
    public List<MonthlyValues> MonthlyValues { get; set; }
}

public class SubCategoryValue
{
    public string SubCategoryName { get; set; }
    public List<MonthlyValues> MonthlyValues { get; set; }
}

public class MonthlyValues
{
    public int Amount { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
}

public class CategoryFlatValue
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
    private readonly ILogger<IndexModel> _logger;

    public YearlyExpensesModel(ApplicationDbContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    [BindProperty]
    public YearlyByCategory ExpensesBySubCategory { get; set; } = default!;

    public async Task OnGetAsync(int? Year)
    {
        int year = Year ?? DateTime.Now.Year;

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        List<Expense> Expenses = null;

        if (_context.Expenses != null)
        {
            Expenses = await _context.Expenses.Where(p => p.UserId == userId && p.Date.Year == year)
            .Include(s => s.CategoryType)
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

        //Transform Expenses into YearlyByCategory
        var result = Expenses.GroupBy(p => new { p.Date.Month, subCategory = p.CategoryType?.Name ?? "- No Sub Category -", category = p.CategoryType?.Category?.Name ?? "- No Categories -" })
            .Select(s => new CategoryFlatValue()
            {
                Year = year,
                Month = s.Key.Month,
                CategoryName = s.Key.category,
                SubCategoryName = s.Key.subCategory,
                Amount = s.Sum(x => x.Amount)
            }).OrderBy(p => p.CategoryName).ToList();

        ExpensesBySubCategory = new YearlyByCategory();
        ExpensesBySubCategory.Categories = new List<CategoryValue>();

        var categoryValuesList = result.GroupBy(c => new { c.CategoryName }).Select(p =>
        new CategoryValue() { CategoryName = p.Key.CategoryName, MonthlyValues = new List<MonthlyValues>() }).ToList();

        foreach (var category in categoryValuesList)
        {
            category.MonthlyValues.AddRange
                (result.Where(p => p.CategoryName.Equals(category.CategoryName)).GroupBy(p => new { p.Month, p.Year })
                .Select(a => new MonthlyValues()
                {
                    Month = a.Key.Month,
                    Year = a.Key.Year,
                    Amount = a.Sum(x => x.Amount)
                }).ToList());
        }

        foreach (var newCategory in categoryValuesList)
        {
            newCategory.SubCategoryList = new List<SubCategoryValue>();

            //Add sub categories
            foreach (var subCategories in result.Where(p => p.CategoryName.Equals(newCategory.CategoryName)))
            {
                var subCategory = newCategory.SubCategoryList.FirstOrDefault(p => p.SubCategoryName.Equals(subCategories.SubCategoryName));

                if (subCategory == null)
                {
                    subCategory = new SubCategoryValue()
                    {
                        SubCategoryName = subCategories.SubCategoryName
                    };
                    newCategory.SubCategoryList.Add(subCategory);
                }

                subCategory.MonthlyValues ??= new List<MonthlyValues>();

                subCategory.MonthlyValues.Add(new MonthlyValues()
                {
                    Amount = subCategories.Amount,
                    Month = subCategories.Month,
                    Year = subCategories.Year,
                });
            }

            ExpensesBySubCategory.Categories = categoryValuesList;
        }
    }
}