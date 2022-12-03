namespace MoneyPlus.Pages.CategoryTypes;

[Authorize]
public class EditModel : PageModel
{
    private readonly MoneyPlus.Data.ApplicationDbContext _context;

    public EditModel(MoneyPlus.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public CategoryType CategoryType { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.CategoryTypes == null)
        {
            return NotFound();
        }

        var categorytype =  await _context.CategoryTypes.FirstOrDefaultAsync(m => m.Id == id);
        if (categorytype == null)
        {
            return NotFound();
        }
        CategoryType = categorytype;
       ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(CategoryType).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CategoryTypeExists(CategoryType.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool CategoryTypeExists(int id)
    {
      return _context.CategoryTypes.Any(e => e.Id == id);
    }
}
