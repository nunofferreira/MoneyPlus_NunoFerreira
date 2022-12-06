namespace MoneyPlus.Pages.Categories;

//TODO[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly MoneyPlus.Data.ApplicationDbContext _context;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(MoneyPlus.Data.ApplicationDbContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IList<Category> Category { get;set; } = default!;

    public async Task OnGetAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (_context.Categories != null)
        {
            Category = await _context.Categories.ToListAsync();
        }
    }
}
