﻿namespace MoneyPlus.Pages.SalesPage;

[Authorize]
public class EditModel : PageModel
{
    private readonly MoneyPlus.Data.ApplicationDbContext _context;

    public EditModel(MoneyPlus.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Expense Expenses { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Expenses == null)
        {
            return NotFound();
        }

        var sales =  await _context.Expenses.FirstOrDefaultAsync(m => m.Id == id);
        if (sales == null)
        {
            return NotFound();
        }
        Expenses = sales;
       ViewData["CategoryTypeId"] = new SelectList(_context.CategoryTypes, "Id", "Name");
       ViewData["PayeeId"] = new SelectList(_context.Payees, "Id", "Name");
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

        _context.Attach(Expenses).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SalesExists(Expenses.Id))
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
      return _context.Expenses.Any(e => e.Id == id);
    }
}