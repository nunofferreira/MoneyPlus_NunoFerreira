using MoneyPlus.Data.Entities;

namespace MoneyPlus.Pages.Transactions;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<IndexModel> _logger;

    public DetailsModel(ApplicationDbContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    public Transaction Transaction { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Transactions == null)
        {
            return NotFound();
        }

        var transaction = await _context.Transactions.FirstOrDefaultAsync(m => m.Id == id);
        transaction.Wallet = _context.Wallets.FirstOrDefault(c => c.Id == transaction.WalletId);

        if (transaction == null)
        {
            return NotFound();
        }
        else
        {
            Transaction = transaction;
        }
        return Page();
    }
}