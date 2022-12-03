namespace MoneyPlus.Pages.Wallets;

public class DetailsModel : PageModel
{
    private readonly MoneyPlus.Data.ApplicationDbContext _context;

    public DetailsModel(MoneyPlus.Data.ApplicationDbContext context)
    {
        _context = context;
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
