namespace MoneyPlus.Pages.CategoryTypes;

//TODO[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly MoneyPlus.Data.ApplicationDbContext _context;

    public IndexModel(MoneyPlus.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<CategoryType> CategoryType { get;set; } = default!;

    public async Task OnGetAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (_context.CategoryTypes != null)
        {
            CategoryType = await _context.CategoryTypes
            .Include(c => c.Category).ToListAsync();
        }
    }
}
