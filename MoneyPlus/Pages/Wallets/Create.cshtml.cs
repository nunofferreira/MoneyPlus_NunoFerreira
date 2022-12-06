namespace MoneyPlus.Pages.Wallets;

[Authorize]
public class CreateModel : PageModel
{
    private readonly MoneyPlus.Data.ApplicationDbContext _context;

    public CreateModel(MoneyPlus.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        ViewData["UserId"] = new SelectList(_context.Users.Where(u => u.UserName == userId), "Id", "Id");
        //ViewData["Wallet"] = new SelectList(_context.Wallets.Where(w => w.UserId == userId), "Id", "Balance");
        return Page();
    }

    [BindProperty]
    public Wallet Wallet { get; set; }


    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Wallets.Add(Wallet);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
