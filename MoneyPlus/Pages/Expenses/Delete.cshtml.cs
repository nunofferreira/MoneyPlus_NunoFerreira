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
            using var dbContextTransaction = _context.Database.BeginTransaction();
            {
                Expenses = sales;
                if (sales.TransactionId > 0)
                {
                    var trans = await _context.Transactions.FindAsync(sales.TransactionId);
                    if(trans != null) _context.Transactions.Remove(trans);
                }
                _context.Expenses.Remove(Expenses);
                await _context.SaveChangesAsync();

                dbContextTransaction.Commit();
            }
        }

        return RedirectToPage("./Index");
    }
}
