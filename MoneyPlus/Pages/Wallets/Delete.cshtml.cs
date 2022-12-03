namespace MoneyPlus.Pages.Wallets;

public class DeleteModel : PageModel
{
    private readonly MoneyPlus.Data.ApplicationDbContext _context;

    public DeleteModel(MoneyPlus.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
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

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null || _context.Wallets == null)
        {
            return NotFound();
        }
        var wallet = await _context.Wallets.FindAsync(id);

        if (wallet != null)
        {
            Wallet = wallet;
            _context.Wallets.Remove(Wallet);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
