﻿namespace MoneyPlus.Pages.TransactionsPage;

public class EditModel : PageModel
{
    private readonly MoneyPlus.Data.ApplicationDbContext _context;

    public EditModel(MoneyPlus.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Transaction Transaction { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Transactions == null)
        {
            return NotFound();
        }

        var transaction =  await _context.Transactions.FirstOrDefaultAsync(m => m.Id == id);
        if (transaction == null)
        {
            return NotFound();
        }
        Transaction = transaction;
       ViewData["WalletId"] = new SelectList(_context.Wallets, "Id", "Id");
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

        _context.Attach(Transaction).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TransactionExists(Transaction.Id))
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

    private bool TransactionExists(int id)
    {
      return _context.Transactions.Any(e => e.Id == id);
    }
}
