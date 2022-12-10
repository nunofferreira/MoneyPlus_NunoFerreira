namespace MoneyPlus.Pages.Assets;

[Authorize]
public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ApplicationDbContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IList<Asset> Asset { get;set; } = default!;

    public async Task OnGetAsync()
    {
        if (_context.Assets != null)
        {
            Asset = await _context.Assets.ToListAsync();
        }
    }
}
