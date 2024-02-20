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
    }
}
