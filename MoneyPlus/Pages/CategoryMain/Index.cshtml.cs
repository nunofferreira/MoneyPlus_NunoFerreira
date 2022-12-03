namespace MoneyPlus.Pages.CategoryMain;

[Authorize]
public class IndexModel : PageModel
{
    private readonly MoneyPlus.Data.ApplicationDbContext _context;

    public IndexModel(MoneyPlus.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Category> Category { get;set; } = default!;

    public async Task OnGetAsync()
    {
        if (_context.Categories != null)
        {
            Category = await _context.Categories.ToListAsync();
        }
    }
}
