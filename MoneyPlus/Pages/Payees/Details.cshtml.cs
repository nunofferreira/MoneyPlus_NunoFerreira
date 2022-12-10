namespace MoneyPlus.Pages.Payees;

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
