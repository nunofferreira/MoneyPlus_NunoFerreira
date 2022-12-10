namespace MoneyPlus.Pages.CategoryTypes;

//TODO[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ApplicationDbContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
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
