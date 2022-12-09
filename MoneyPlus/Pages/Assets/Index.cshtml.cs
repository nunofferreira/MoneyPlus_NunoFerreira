namespace MoneyPlus.Pages.Assets;

[Authorize]
public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
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
