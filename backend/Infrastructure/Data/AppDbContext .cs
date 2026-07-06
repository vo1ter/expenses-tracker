using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = "Host=193.23.218.81; Port=5432; Username=dev_usr; Password=j0medVbThJQjP0hq; Database=exp_track_dev";
        optionsBuilder.UseNpgsql(connectionString);
    }

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
