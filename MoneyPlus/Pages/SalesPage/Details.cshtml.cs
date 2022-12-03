namespace MoneyPlus.Pages.SalesPage;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly MoneyPlus.Data.ApplicationDbContext _context;

    public DetailsModel(MoneyPlus.Data.ApplicationDbContext context)
    {
        _context = context;
    }

  public Sales Sales { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Sales == null)
        {
            return NotFound();
        }

        var sales = await _context.Sales.FirstOrDefaultAsync(m => m.Id == id);
        if (sales == null)
        {
            return NotFound();
        }
        else 
        {
            Sales = sales;
        }
        return Page();
    }
}
