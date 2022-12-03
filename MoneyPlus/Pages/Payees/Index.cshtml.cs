namespace MoneyPlus.Pages.Payees;

[Authorize]
public class IndexModel : PageModel
{
    private readonly MoneyPlus.Data.ApplicationDbContext _context;

    public IndexModel(MoneyPlus.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Payee> Payee { get;set; } = default!;

    public async Task OnGetAsync()
    {
        if (_context.Payees != null)
        {
            Payee = await _context.Payees.ToListAsync();
        }
    }
}
