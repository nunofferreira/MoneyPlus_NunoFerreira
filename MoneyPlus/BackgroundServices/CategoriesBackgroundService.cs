namespace MoneyPlus.BackgroundServices;

public class CategoriesBackgroundService : BackgroundService
{
    private readonly ILogger<CategoriesBackgroundService> _logger;

    // Configurations
    readonly TimeSpan IntervalBeweenJobs = TimeSpan.FromMinutes(5);

    public IServiceProvider _serviceProvider { get; }

    public CategoriesBackgroundService(IServiceProvider serviceProvider, ILogger<CategoriesBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        while (true)
        {
            await DoWorkAsync(context);
            
            Debug.WriteLine(context.CategoryTypes.ToList());

            await Task.Delay(IntervalBeweenJobs);
        }
    }

    private async Task DoWorkAsync(ApplicationDbContext context)
    {
        try
        {
            string path = @"c:\temp\Categorias";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            DataBase fileDB = new()
            {
                CategoryList = context.Categories.Select(p => p.Name).Distinct().ToList(),
                SubCategoryList = context.CategoryTypes.Select(p => p.Name).Distinct().ToList()
            };

            fileDB.Serialize(Path.Combine(path, $"catList_{DateTime.Now.ToFileTime()}.yaml"));

        }
        catch (Exception ex)
        {
            _logger.LogError("Error executing background service...", ex);
        }
    }
}