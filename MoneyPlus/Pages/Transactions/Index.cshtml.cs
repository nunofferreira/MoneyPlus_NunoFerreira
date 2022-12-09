namespace MoneyPlus.Pages.Transactions;

[Authorize]
public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
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
