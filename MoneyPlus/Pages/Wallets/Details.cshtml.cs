﻿namespace MoneyPlus.Pages.Wallets;

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

    public Wallet Wallet { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Wallets == null)
        {
            return NotFound();
        }

        var wallet = await _context.Wallets.FirstOrDefaultAsync(m => m.Id == id);
        if (wallet == null)
        {
            return NotFound();
        }
        else
        {
            Wallet = wallet;
        }
        return Page();
    }
}