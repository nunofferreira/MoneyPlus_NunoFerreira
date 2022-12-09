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

    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        List<Asset> assets = new List<Asset>();
        assets.Add(new Asset() { Id = 0, Name = "- No asset -" });
        assets.AddRange(_context.Assets);

        ViewData["Date"] = DateTime.UtcNow;
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        ViewData["CategoryTypeId"] = new SelectList(_context.CategoryTypes, "Id", "Name");
        ViewData["PayeeId"] = new SelectList(_context.Payees, "Id", "Name");
        ViewData["TransactionId"] = new SelectList(_context.Transactions, "Id", "Id");
        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
        ViewData["AssetId"] = new SelectList(assets, "Id", "Name");
        ViewData["PaymentType"] = new SelectList(PayMethods);
        ViewData["Wallet"] = new SelectList(_context.Wallets.Where(w => w.UserId == userId), "Id", "Balance");
        return Page();
    }

    [BindProperty]
    public Expense Expenses { get; set; }

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
            if (Expenses.PaymentMethod.Equals("MB") || Expenses.PaymentMethod.Equals("Transfer"))
            {
                newTran.Amount = Expenses.Amount;
                newTran.Date = Expenses.Date;
                newTran.Type = TransactionType.Debit;
                newTran.TransactionType = Expenses.PaymentMethod;
                newTran.Wallet = _context.Wallets.FirstOrDefault(x => x.Id == Expenses.WalletId);
                _context.Transactions.Add(newTran);

                await _context.SaveChangesAsync();
            }

            Expenses.UserId = userId;

            if (newTran != null)
            {
                Wallet wallet = _context.Wallets.FirstOrDefault(x => x.Id == newTran.WalletId);
                wallet.Balance -= newTran.Amount;

                if (wallet.Balance < 0)
                {
                    throw new Exception("You have no balance");
                }

                Expenses.TransactionId = newTran.Id;
            }

            if (Expenses.AssetId == 0)
            {
                Expenses.AssetId = null;
            }

            _context.Expenses.Add(Expenses);
            await _context.SaveChangesAsync();

            dbContextTransaction.Commit();
        }

        return RedirectToPage("./Index");
    }
}