using DailyExpenseAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DailyExpenseAPI.Data
{
    public class DailyExpenseDbContext : DbContext
    {
        public DailyExpenseDbContext(DbContextOptions<DailyExpenseDbContext> options) : base(options)
        {

        }

        public DbSet<Expense> Expenses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expense>();
        }
    }
}
