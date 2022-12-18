namespace MoneyPlus.Pages.CategoryTypes;

[Authorize(Roles = "Admin")]
public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<IndexModel> _logger;

    public CreateModel(ApplicationDbContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult OnGet()
    {
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
        return Page();
    }

    [BindProperty]
    public CategoryType CategoryType { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.CategoryTypes.Add(CategoryType);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}