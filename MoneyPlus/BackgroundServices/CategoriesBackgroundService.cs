namespace MoneyPlus.BackgroundServices;

public class CategoriesBackgroundService : BackgroundService
{
    // Configurations
    TimeSpan IntervalBeweenJobs = TimeSpan.FromMinutes(5);

    public IServiceProvider _serviceProvider { get; }

    public CategoriesBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }


    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        using var scope = _serviceProvider.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        while (true)
        {
            await DoWorkAsync();

            Debug.WriteLine(ctx.CategoryTypes.ToList()) ;

            await Task.Delay(IntervalBeweenJobs);
        }
    }

    private async Task DoWorkAsync()
    {
        try
        {
            // Código que deve ser executado
        }
        catch (Exception ex)
        {

        }
    }
}
