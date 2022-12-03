namespace MoneyPlus.Pages.Payees;

[Authorize]
public class DeleteModel : PageModel
{
    private readonly MoneyPlus.Data.ApplicationDbContext _context;

    public DeleteModel(MoneyPlus.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
  public Payee Payee { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Payees == null)
        {
            return NotFound();
        }

        var payee = await _context.Payees.FirstOrDefaultAsync(m => m.Id == id);

        if (payee == null)
        {
            return NotFound();
        }
        else 
        {
            Payee = payee;
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null || _context.Payees == null)
        {
            return NotFound();
        }
        var payee = await _context.Payees.FindAsync(id);

        if (payee != null)
        {
            Payee = payee;
            _context.Payees.Remove(Payee);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
