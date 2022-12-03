namespace MoneyPlus.Pages.CategoryTypes;

[Authorize]
public class DeleteModel : PageModel
{
    private readonly MoneyPlus.Data.ApplicationDbContext _context;

    public DeleteModel(MoneyPlus.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
  public CategoryType CategoryType { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.CategoryTypes == null)
        {
            return NotFound();
        }

        var categorytype = await _context.CategoryTypes.FirstOrDefaultAsync(m => m.Id == id);

        if (categorytype == null)
        {
            return NotFound();
        }
        else 
        {
            CategoryType = categorytype;
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null || _context.CategoryTypes == null)
        {
            return NotFound();
        }
        var categorytype = await _context.CategoryTypes.FindAsync(id);

        if (categorytype != null)
        {
            CategoryType = categorytype;
            _context.CategoryTypes.Remove(CategoryType);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
