namespace MoneyPlus.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ApplicationDbContext _ctx;

    public IndexModel(ILogger<IndexModel> logger,
        ApplicationDbContext ctx)
    {
        _logger = logger;
        _ctx = ctx;
    }

   

    public async Task OnGetAsync()
    {
        _logger.LogWarning($"Executing Index");
        _logger.LogCritical("Executing Index");
        _logger.LogInformation("Executing Index");
    }
}