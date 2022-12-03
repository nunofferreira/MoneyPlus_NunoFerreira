namespace MoneyPlus.Pages.SalesPage;

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
    ViewData["CategoryTypeId"] = new SelectList(_context.CategoryTypes, "Id", "Id");
    ViewData["PayeeId"] = new SelectList(_context.Payees, "Id", "Id");
    ViewData["TransactionId"] = new SelectList(_context.Transactions, "Id", "Id");
    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
        return Page();
    }

    [BindProperty]
    public Sales Sales { get; set; }
    

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Sales.Add(Sales);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
