namespace MoneyPlus.Pages.SalesPage;

[Authorize]
public class IndexModel : PageModel
{
    private readonly MoneyPlus.Data.ApplicationDbContext _context;

    public IndexModel(MoneyPlus.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Sales> Sales { get;set; } = default!;

    public async Task OnGetAsync()
    {
        if (_context.Sales != null)
        {
            Sales = await _context.Sales
            .Include(s => s.CategoryType)
            .Include(s => s.Payee)
            .Include(s => s.Transaction)
            .Include(s => s.User).ToListAsync();
        }
    }
}
