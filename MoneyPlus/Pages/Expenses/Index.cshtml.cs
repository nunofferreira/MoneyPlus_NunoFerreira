namespace MoneyPlus.Pages.SalesPage;

[Authorize]
public class IndexModel : PageModel
{
    private readonly MoneyPlus.Data.ApplicationDbContext _context;

    public IndexModel(MoneyPlus.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Expense> Expenses { get; set; } = default!;

    public async Task OnGetAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (_context.Expenses != null)
        {
            Expenses = await _context.Expenses
            .Include(s => s.CategoryType)
            .Include(s => s.Payee)
            .Include(s => s.Transaction)
            .Include(s => s.User)
            .Include(s => s.Asset)
            .Where(u => u.UserId == userId)
            .ToListAsync();
        }
    }
}
