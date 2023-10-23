using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context;

public class BankAppDbContext : DbContext
{
    public BankAppDbContext(DbContextOptions<BankAppDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer(
            "Server=localhost;Database=Bank_Project;User Id=sa;password=myPassw0rd;TrustServerCertificate=True;");
    }
}