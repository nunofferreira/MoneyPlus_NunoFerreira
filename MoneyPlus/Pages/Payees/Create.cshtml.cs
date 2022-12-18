namespace MoneyPlus.Pages.Payees;

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
    public Payee Payee { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Payee.UserId = userId;

        _context.Payees.Add(Payee);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}