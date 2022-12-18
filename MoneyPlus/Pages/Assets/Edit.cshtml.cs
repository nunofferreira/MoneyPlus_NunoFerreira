namespace MoneyPlus.Pages.Assets;

[Authorize]
public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<IndexModel> _logger;

    public EditModel(ApplicationDbContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    [BindProperty]
    public Asset Asset { get; set; } = default!;

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
        Asset = asset;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(Asset).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AssetExists(Asset.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool AssetExists(int id)
    {
        return _context.Assets.Any(e => e.Id == id);
    }
}