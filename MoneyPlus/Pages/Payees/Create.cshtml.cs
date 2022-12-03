namespace MoneyPlus.Pages.Payees;

public class CreateModel : PageModel
{
    private readonly MoneyPlus.Data.ApplicationDbContext _context;

    public CreateModel(MoneyPlus.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public Payee Payee { get; set; }
    

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Payees.Add(Payee);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
