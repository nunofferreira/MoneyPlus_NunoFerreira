namespace MoneyPlus.Pages.Transactions;

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

    public IList<Transaction> Transaction { get; set; } = default!;

    public async Task OnGetAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (_context.Transactions != null)
        {
            Transaction = await _context.Transactions
            .Where(t => t.Wallet.UserId == userId)
            .Include(t => t.Wallet).ToListAsync();
        }
    }
}
