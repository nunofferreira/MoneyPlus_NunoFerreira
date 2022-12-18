namespace MoneyPlus.Pages.Transactions;

[Authorize]
public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<IndexModel> _logger;

    public CreateModel(ApplicationDbContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult OnGet()
    {
        ViewData["WalletId"] = new SelectList(_context.Wallets, "Id", "Name");
        ViewData["Type"] = new SelectList(Enum.GetValues(typeof(TransactionType)));
        return Page();
    }

    [BindProperty]
    public Transaction Transaction { get; set; }

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