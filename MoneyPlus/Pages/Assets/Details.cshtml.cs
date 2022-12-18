namespace MoneyPlus.Pages.Assets;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<IndexModel> _logger;

    public DetailsModel(ApplicationDbContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    public Asset Asset { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Assets == null)
        {
            return NotFound();
        }

        var asset = await _context.Assets.FirstOrDefaultAsync(m => m.Id == id);
        if (asset == null)
        {
            return NotFound();
        }
        else
        {
            Asset = asset;
        }
        return Page();
    }
}