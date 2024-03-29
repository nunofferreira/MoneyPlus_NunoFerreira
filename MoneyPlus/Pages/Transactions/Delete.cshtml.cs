﻿namespace MoneyPlus.Pages.Transactions;

[Authorize]
public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<IndexModel> _logger;

    public DeleteModel(ApplicationDbContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    [BindProperty]
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

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null || _context.Transactions == null)
        {
            return NotFound();
        }
        var transaction = await _context.Transactions.FindAsync(id);

        if (transaction != null)
        {
            Transaction = transaction;
            _context.Transactions.Remove(Transaction);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}