namespace MoneyPlus.Pages.CategoryMain;

public class DeleteModel : PageModel
{
    private readonly MoneyPlus.Data.ApplicationDbContext _context;

    public DeleteModel(MoneyPlus.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
  public Category Category { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Categories == null)
        {
            return NotFound();
        }

        var category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);

        if (category == null)
        {
            return NotFound();
        }
        else 
        {
            Category = category;
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null || _context.Categories == null)
        {
            return NotFound();
        }
        var category = await _context.Categories.FindAsync(id);

        if (category != null)
        {
            Category = category;
            _context.Categories.Remove(Category);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
