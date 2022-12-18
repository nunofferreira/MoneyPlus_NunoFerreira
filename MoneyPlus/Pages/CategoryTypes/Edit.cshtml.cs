namespace MoneyPlus.Pages.CategoryTypes;

[Authorize(Roles = "Admin")]
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
    public CategoryType CategoryType { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.CategoryTypes == null)
        {
            return NotFound();
        }

        var categorytype = await _context.CategoryTypes.FirstOrDefaultAsync(m => m.Id == id);
        if (categorytype == null)
        {
            return NotFound();
        }
        CategoryType = categorytype;
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(CategoryType).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CategoryTypeExists(CategoryType.Id))
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

    private bool CategoryTypeExists(int id)
    {
        return _context.CategoryTypes.Any(e => e.Id == id);
    }
}