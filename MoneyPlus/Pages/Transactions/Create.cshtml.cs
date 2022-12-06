namespace MoneyPlus.Pages.Transactions;

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
        ViewData["WalletId"] = new SelectList(_context.Wallets, "Id", "Name");
        return Page();
    }

    [BindProperty]
    public Transaction Transaction { get; set; }


    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Transactions.Add(Transaction);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
