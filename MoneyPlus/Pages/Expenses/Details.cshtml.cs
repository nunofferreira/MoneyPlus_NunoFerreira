namespace MoneyPlus.Pages.Expenses;

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

    public Expense Expenses { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Expenses == null)
            return NotFound();

        var expenses = await _context.Expenses.FirstOrDefaultAsync(m => m.Id == id);
        expenses.Asset = _context.Assets.FirstOrDefault(m => m.Id == expenses.AssetId);
        expenses.Payee = _context.Payees.FirstOrDefault(m => m.Id == expenses.PayeeId);

        if (expenses == null)
            return NotFound();
        else
            Expenses = expenses;
        return Page();
    }
}