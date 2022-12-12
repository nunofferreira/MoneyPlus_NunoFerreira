namespace MoneyPlus.Pages.Assets;

[Authorize]
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
    public Asset Asset { get; set; }
    

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
        {
            return Page();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Asset.UserId = userId;

        _context.Assets.Add(Asset);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
