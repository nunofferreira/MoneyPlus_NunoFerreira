namespace MoneyPlus.Pages.SalesPage;

[Authorize]
public class CreateModel : PageModel
{
    public List<string> PayMethods
    {
        get => new()
        {
        "MB",
        "Money",
        "Transfer"
        };
    }

    private readonly MoneyPlus.Data.ApplicationDbContext _context;

    public CreateModel(MoneyPlus.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        ViewData["Date"] = DateTime.UtcNow;
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        ViewData["CategoryTypeId"] = new SelectList(_context.CategoryTypes, "Id", "Name");
        ViewData["PayeeId"] = new SelectList(_context.Payees, "Id", "Name");
        ViewData["TransactionId"] = new SelectList(_context.Transactions, "Id", "Id");
        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
        ViewData["PaymentType"] = new SelectList(PayMethods);
        ViewData["Wallet"] = new SelectList(_context.Wallets.Where(w => w.UserId == userId), "Id", "Balance");
        return Page();
    }

    [BindProperty]
    public Sales Sales { get; set; }

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!ModelState.IsValid)
        {
            return Page();
        }

        Transaction newTran = new();

        using var dbContextTransaction = _context.Database.BeginTransaction();
        {
            if (Sales.PaymentMethod.Equals("MB") || Sales.PaymentMethod.Equals("Transfer"))
            {
                newTran.Amount = Sales.Amount;
                newTran.Date = Sales.Date;
                newTran.Type = "D";
                newTran.TransactionType = Sales.PaymentMethod;
                newTran.Wallet = _context.Wallets.FirstOrDefault(x => x.Id == Sales.WalletId);
                _context.Transactions.Add(newTran);

                await _context.SaveChangesAsync();
            }

            Sales.UserId = userId;

            if (newTran != null)
            {
                Wallet wallet = _context.Wallets.FirstOrDefault(x => x.Id == newTran.WalletId);
                wallet.Balance -= newTran.Amount;

                if (wallet.Balance < 0)
                {
                    throw new Exception("You have no balance");
                }

                Sales.TransactionId = newTran.Id;
            }

            _context.Sales.Add(Sales);
            await _context.SaveChangesAsync();

            dbContextTransaction.Commit();
        }

        return RedirectToPage("./Index");
    }
}