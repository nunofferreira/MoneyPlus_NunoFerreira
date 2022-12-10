namespace MoneyPlus.Pages.Expenses;

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
    public Expense Expenses { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Expenses == null)
        {
            return NotFound();
        }

        var sales = await _context.Expenses.FirstOrDefaultAsync(m => m.Id == id);

        if (sales == null)
        {
            return NotFound();
        }
        else
        {
            Expenses = sales;
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null || _context.Expenses == null)
        {
            return NotFound();
        }
        var sales = await _context.Expenses.FindAsync(id);

        if (sales != null)
        {
            Expenses = sales;
            _context.Expenses.Remove(Expenses);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
