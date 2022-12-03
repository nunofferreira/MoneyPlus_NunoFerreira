namespace MoneyPlus.Pages.SalesPage;

[Authorize]
public class DeleteModel : PageModel
{
    private readonly MoneyPlus.Data.ApplicationDbContext _context;

    public DeleteModel(MoneyPlus.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
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

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null || _context.Sales == null)
        {
            return NotFound();
        }
        var sales = await _context.Sales.FindAsync(id);

        if (sales != null)
        {
            Sales = sales;
            _context.Sales.Remove(Sales);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
