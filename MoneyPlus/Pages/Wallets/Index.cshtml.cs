namespace MoneyPlus.Pages.Wallets;

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

    public IList<Wallet> Wallet { get; set; } = default!;

    public async Task OnGetAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (_context.Wallets != null)
        {
            Wallet = await _context.Wallets
            .Where(w => w.UserId == userId).ToListAsync();
        }
    }
}
