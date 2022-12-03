namespace MoneyPlus.Pages.SalesPage;

[Authorize]
public class EditModel : PageModel
{
    private readonly MoneyPlus.Data.ApplicationDbContext _context;

    public EditModel(MoneyPlus.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Sales Sales { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Sales == null)
        {
            return NotFound();
        }

        var sales =  await _context.Sales.FirstOrDefaultAsync(m => m.Id == id);
        if (sales == null)
        {
            return NotFound();
        }
        Sales = sales;
       ViewData["CategoryTypeId"] = new SelectList(_context.CategoryTypes, "Id", "Id");
       ViewData["PayeeId"] = new SelectList(_context.Payees, "Id", "Id");
       ViewData["TransactionId"] = new SelectList(_context.Transactions, "Id", "Id");
       ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
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

        _context.Attach(Sales).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SalesExists(Sales.Id))
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

    private bool SalesExists(int id)
    {
      return _context.Sales.Any(e => e.Id == id);
    }
}
