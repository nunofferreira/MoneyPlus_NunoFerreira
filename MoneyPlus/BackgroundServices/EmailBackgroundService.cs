namespace MoneyPlus.BackgroundServices;

public class EmailBackgroundService : BackgroundService
{
    // Configurations
    TimeSpan IntervalBeweenJobs = TimeSpan.FromDays(1);

    public IServiceProvider _serviceProvider { get; }

    public EmailBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {

        while (!ct.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            SendEmail();

            await Task.Delay(IntervalBeweenJobs);
        }
    }

    private void SendEmail()
    {
        try
        {
            // Código que deve ser executado
            Debug.WriteLine($"Sending email at: {DateTime.UtcNow}");
        }
        catch (Exception ex)
        {

        }
    }
}
