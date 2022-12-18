namespace MoneyPlus.Pages.CategoryTypes;

[Authorize(Roles = "Admin")]
public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<IndexModel> _logger;

    public DeleteModel(ApplicationDbContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    [BindProperty]
    public CategoryType CategoryType { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.CategoryTypes == null)
        {
            return NotFound();
        }

        var categorytype = await _context.CategoryTypes.FirstOrDefaultAsync(m => m.Id == id);
        categorytype.Category = _context.Categories.FirstOrDefault(c => c.Id == categorytype.CategoryId);

        if (categorytype == null)
        {
            return NotFound();
        }
        else
        {
            CategoryType = categorytype;
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null || _context.CategoryTypes == null)
        {
            return NotFound();
        }
        var categorytype = await _context.CategoryTypes.FindAsync(id);

        if (categorytype != null)
        {
            CategoryType = categorytype;
            _context.CategoryTypes.Remove(CategoryType);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
