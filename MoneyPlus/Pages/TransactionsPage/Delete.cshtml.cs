namespace MoneyPlus.Pages.TransactionsPage;

public class DeleteModel : PageModel
{
    private readonly MoneyPlus.Data.ApplicationDbContext _context;

    public DeleteModel(MoneyPlus.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
  public Transaction Transaction { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Transactions == null)
        {
            return NotFound();
        }

        var transaction = await _context.Transactions.FirstOrDefaultAsync(m => m.Id == id);

        if (transaction == null)
        {
            return NotFound();
        }
        else 
        {
            Transaction = transaction;
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null || _context.Transactions == null)
        {
            return NotFound();
        }
        var transaction = await _context.Transactions.FindAsync(id);

        if (transaction != null)
        {
            Transaction = transaction;
            _context.Transactions.Remove(Transaction);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
