using Microsoft.EntityFrameworkCore;

namespace ExpensesApi.Models
{
    public class SpendSmartDbContext : DbContext
    {
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Budget> Budgets { get; set; }

        public SpendSmartDbContext(DbContextOptions<SpendSmartDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // one budget per category
            modelBuilder.Entity<Budget>()
                .HasIndex(b => b.Category)
                .IsUnique();
        }
    }
}