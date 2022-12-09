﻿namespace MoneyPlus.Pages.Wallets;

[Authorize]
public class IndexModel : PageModel
{
    private readonly MoneyPlus.Data.ApplicationDbContext _context;

    public IndexModel(MoneyPlus.Data.ApplicationDbContext context)
    {
        _context = context;
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