namespace MoneyPlus.BackgroundServices;

public class EmailBackgroundService : BackgroundService
{
    // Configurations
    readonly TimeSpan IntervalBeweenJobs = TimeSpan.FromDays(1);
    private readonly ILogger<EmailBackgroundService> _logger;
    public IServiceProvider _serviceProvider { get; }

    public EmailBackgroundService(IServiceProvider serviceProvider, ILogger<EmailBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            SendEmail();

            await Task.Delay(IntervalBeweenJobs, ct);
        }
    }

    private void SendEmail()
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            _logger.LogInformation("Starting email sending ...");

            var walletByUser = context.Wallets.GroupBy(p => p.UserId)
                .Select(p => new { UserId = p.Key, Total = p.Sum(w => w.Balance) }).ToList();

            foreach (var userBalance in walletByUser)
            {
                var userInfo = context.Users.FirstOrDefault(p => p.Id.Equals(userBalance.UserId));

                string emailBody = $@"Hello {userInfo.UserName}.
                Your current balance is: {String.Format("{0:C}", userBalance.Total)}";

                EmailMessage newMessage = new()
                {
                    To = userInfo.Email,
                    Body = emailBody,
                    Subject = $"Balance at {DateTime.Now.ToShortDateString()}"
                };

                try
                {
                    SendEmailToUser(context, newMessage);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error sending email to user {userInfo.Email}...", ex);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Error sending email...", ex);
        }
    }

    private void SendEmailToUser(ApplicationDbContext context, EmailMessage message)
    {
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("Insert your email", "Insert your password"),
            EnableSsl = true,
        };
        
        // Activate bellow with relevant credentials

        //smtpClient.Send("Insert your email", message.To, message.Subject, message.Body);
    }
}