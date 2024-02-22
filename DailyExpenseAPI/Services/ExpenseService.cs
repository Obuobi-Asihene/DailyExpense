using DailyExpenseAPI.Data;
using DailyExpenseAPI.Models;
using DailyExpenseAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace DailyExpenseAPI.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly DailyExpenseDbContext _context;

        public ExpenseService(DailyExpenseDbContext context)
        {
            _context = context;
        }
        public async Task AddExpenseAsync(Expense expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteExpenseAsync(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Expense>> GetAllExpnsesAsync()
        {
            return await _context.Expenses.ToListAsync();
        }

        public async Task<Expense> GetExpenseByIdAsync(int id)
        {
            return await _context.Expenses.FindAsync(id);
        }

        public async Task UpdateExpenseAsync(Expense expense)
        {
            _context.Entry(expense).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<DailyExpense>> CalculateDailyExpenseAsync()
        {
            //query db and retrieve expense
            var expenses = await _context.Expenses.ToListAsync();

            //group expense by date and calculate total
            var dailyExpenses = expenses
                .GroupBy(e => e.Date.Date)
                .Select(g => new DailyExpense
                {
                    Date = g.Key,
                    TotalExpense = g.Sum(e => e.Price)
                })
                .OrderBy(de => de.Date);

            return dailyExpenses;
        }

        public async Task CreateDailyExpenseRecordsAsync()
        {
            // Get today's date
            DateTime today = DateTime.Today;

            // Check if expense records already exist for today
            bool recordsExist = await _context.Expenses.AnyAsync(e => e.Date.Date == today);

            // If records don't exist, create a new table for today's expenses and move today's expenses to that table
            if (!recordsExist)
            {
                // Create a new table with today's date
                string tableName = $"Expenses_{today:yyyyMMdd}";
                await _context.Database.ExecuteSqlRawAsync($@"
                    CREATE TABLE {tableName} (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        ItemName VARCHAR(MAX) NOT NULL,
                        Price DECIMAL(18,2) NOT NULL,
                        Date DATE NOT NULL
                    )");

                // Move today's expenses to the new table
                await _context.Database.ExecuteSqlRawAsync($@"
                    INSERT INTO {tableName} (ItemName, Price, Date)
                    SELECT ItemName, Price, Date
                    FROM Expenses
                    WHERE Date = '{today:yyyy-MM-dd}'");

                // Remove today's expenses from the main Expenses table
                var todaysExpenses = await _context.Expenses.Where(e => e.Date.Date == today).ToListAsync();
                _context.Expenses.RemoveRange(todaysExpenses);

                // Save changes to the database
                await _context.SaveChangesAsync();
            }
        }
    }
}
