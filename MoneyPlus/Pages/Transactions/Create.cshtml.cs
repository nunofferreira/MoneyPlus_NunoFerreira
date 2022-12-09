using Microsoft.EntityFrameworkCore.Query.Internal;

namespace MoneyPlus.Pages.Transactions;

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
        ViewData["WalletId"] = new SelectList(_context.Wallets, "Id", "Name");
        ViewData["Type"] = new SelectList(Enum.GetValues(typeof(TransactionType)));
        return Page();
    }

    [BindProperty]
    public Transaction Transaction { get; set; }


    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        using var dbContextTransaction = _context.Database.BeginTransaction();
        {

            Wallet wallet = _context.Wallets.FirstOrDefault(x => x.Id == Transaction.WalletId);
            if (Transaction.Type == TransactionType.Credit)
            {

                wallet.Balance += Transaction.Amount;
            }
            else
            {
                wallet.Balance -= Transaction.Amount;
            }
            if (wallet.Balance < 0)
            {
                throw new Exception("You have no balance");
            }

            _context.Transactions.Add(Transaction);

            await _context.SaveChangesAsync();

            dbContextTransaction.Commit();
        }

        return RedirectToPage("./Index");
    }
}
