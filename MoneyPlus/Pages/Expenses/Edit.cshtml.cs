namespace MoneyPlus.Pages.Expenses;

[Authorize]
public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<IndexModel> _logger;

    public EditModel(ApplicationDbContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    [BindProperty]
    public Expense Expenses { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        List<Asset> assets = new List<Asset>();
        assets.Add(new Asset() { Id = 0, Name = "- No asset -" });
        assets.AddRange(_context.Assets);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (id == null || _context.Expenses == null)
        {
            return NotFound();
        }

        var sales = await _context.Expenses.FirstOrDefaultAsync(m => m.Id == id);
        if (sales == null)
        {
            return NotFound();
        }
        Expenses = sales;
        ViewData["CategoryTypeId"] = new SelectList(_context.CategoryTypes, "Id", "Name");
        ViewData["PayeeId"] = new SelectList(_context.Payees, "Id", "Name");
        ViewData["TransactionId"] = new SelectList(_context.Transactions, "Id", "Id");
        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
        ViewData["AssetId"] = new SelectList(assets, "Id", "Name");
        ViewData["PaymentType"] = new SelectList(CreateModel.PayMethods);
        ViewData["Wallet"] = new SelectList(_context.Wallets.Where(w => w.UserId == userId), "Id", "Name");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (Expenses.AssetId == 0)
        {
            Expenses.AssetId = null;
        }

        _context.Attach(Expenses).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ExpenseExists(Expenses.Id))
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

    private bool ExpenseExists(int id)
    {
        return _context.Expenses.Any(e => e.Id == id);
    }
}