using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts => Set<Account>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>().ToTable("accounts");

        modelBuilder.Entity<Account>()
            .OwnsOne(a => a.Currency);

        modelBuilder.Entity<Category>().ToTable("categories");
        modelBuilder.Entity<Transaction>().ToTable("transactions");
        modelBuilder.Entity<RecurringTransaction>().ToTable("recurring_transactions");
    }
}