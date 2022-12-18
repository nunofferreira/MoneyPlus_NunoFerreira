namespace MoneyPlus.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryType> CategoryTypes { get; set; }
    public DbSet<Payee> Payees { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Asset> Assets { get; set; }
    public DbSet<EmailMessage> EmailMessages { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}