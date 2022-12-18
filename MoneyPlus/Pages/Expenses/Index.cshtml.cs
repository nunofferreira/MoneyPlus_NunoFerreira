namespace MoneyPlus.Pages.Expenses;

[Authorize]
public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ApplicationDbContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
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