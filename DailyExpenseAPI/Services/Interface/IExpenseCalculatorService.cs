using DailyExpenseAPI.Models;

namespace DailyExpenseAPI.Services.Interface
{
    public interface IExpenseCalculatorService
    {
        Task<IEnumerable<DailyExpense>> CalculateDailyExpenseAsync(IEnumerable<Expense> expenses);
        Task<IEnumerable<WeeklyExpense>> CalculateWeeklyExpenseAsync(IEnumerable<Expense> expense);
        Task<IEnumerable<MonthlyExpense>> CalculateMonthlyExpenseAsync(IEnumerable<Expense> expenses);
    }
}
