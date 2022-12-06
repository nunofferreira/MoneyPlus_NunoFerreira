namespace MoneyPlus.Pages.CategoryTypes;

//TODO[Authorize(Roles = "Admin")]
public class DetailsModel : PageModel
{
    private readonly MoneyPlus.Data.ApplicationDbContext _context;

    public DetailsModel(MoneyPlus.Data.ApplicationDbContext context)
    {
        _context = context;
    }

  public CategoryType CategoryType { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.CategoryTypes == null)
        {
            return NotFound();
        }

        var categorytype = await _context.CategoryTypes.FirstOrDefaultAsync(m => m.Id == id);
        categorytype.Category = _context.Categories.FirstOrDefault(c => c.Id == categorytype.CategoryId);

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
}
