namespace MoneyPlus.Pages.TransactionsPage;

[Authorize]
public class IndexModel : PageModel
{
    private readonly MoneyPlus.Data.ApplicationDbContext _context;

    public IndexModel(MoneyPlus.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Transaction> Transaction { get;set; } = default!;

    public async Task OnGetAsync()
    {
        if (_context.Transactions != null)
        {
            Transaction = await _context.Transactions
            .Include(t => t.Wallet).ToListAsync();
        }
    }
}
