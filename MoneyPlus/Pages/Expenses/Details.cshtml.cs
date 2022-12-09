namespace MoneyPlus.Pages.SalesPage;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly MoneyPlus.Data.ApplicationDbContext _context;

    public DetailsModel(MoneyPlus.Data.ApplicationDbContext context)
    {
        _context = context;
    }

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
}
