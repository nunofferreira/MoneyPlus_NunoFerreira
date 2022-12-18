namespace MoneyPlus.Pages.Categories;

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
        return Page();
    }

    [BindProperty]
    public Category Category { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Categories.Add(Category);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}