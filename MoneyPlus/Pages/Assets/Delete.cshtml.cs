namespace MoneyPlus.Pages.Assets;

[Authorize]
public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
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

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null || _context.Assets == null)
        {
            return NotFound();
        }
        var asset = await _context.Assets.FindAsync(id);

        if (asset != null)
        {
            Asset = asset;
            _context.Assets.Remove(Asset);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
