namespace MoneyPlus.Pages.TransactionsPage;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly MoneyPlus.Data.ApplicationDbContext _context;

    public DetailsModel(MoneyPlus.Data.ApplicationDbContext context)
    {
        _context = context;
    }

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
}
