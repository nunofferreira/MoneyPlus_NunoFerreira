namespace MoneyPlus.Pages.CategoryTypes;

//TODO[Authorize(Roles = "Admin")]
public class CreateModel : PageModel
{
    private readonly MoneyPlus.Data.ApplicationDbContext _context;

    public CreateModel(MoneyPlus.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
        return Page();
    }

    [BindProperty]
    public CategoryType CategoryType { get; set; }
    

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
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
