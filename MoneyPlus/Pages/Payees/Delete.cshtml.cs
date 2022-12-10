namespace MoneyPlus.Pages.Payees;

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
