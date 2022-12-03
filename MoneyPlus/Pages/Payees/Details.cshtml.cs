namespace MoneyPlus.Pages.Payees;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly MoneyPlus.Data.ApplicationDbContext _context;

    public DetailsModel(MoneyPlus.Data.ApplicationDbContext context)
    {
        _context = context;
    }

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
}
