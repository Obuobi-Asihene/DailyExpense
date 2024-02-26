using DailyExpenseAPI.Models;
using DailyExpenseAPI.Services.Interface;
using System.Globalization;

namespace DailyExpenseAPI.Services
{
    public class ExpenseCalculatorService : IExpenseCalculatorService
    {
        public Task<IEnumerable<DailyExpense>> CalculateDailyExpenseAsync(IEnumerable<Expense> expenses)
        {
            var dailyExpenses = expenses
                .GroupBy(e => e.Date.Date)
                .Select(g => new DailyExpense
                {
                    Date = g.Key,
                    TotalExpense = g.Sum(e => e.Price)
                })
                .OrderBy(e => e.Date);

            return Task.FromResult(dailyExpenses.AsEnumerable());
        }

        public Task<IEnumerable<WeeklyExpense>> CalculateWeeklyExpenseAsync(IEnumerable<Expense> expenses)
        {
            var weeklyExpenses = expenses
                .GroupBy(e => CalculateWeekNumber(e.Date))
                .Select(g => new WeeklyExpense
                {
                    WeekNumber = g.Key,
                    TotalWeeklyExpense = g.Sum(e => e.Price)
                })
                .OrderBy(we => we.WeekNumber);

            return Task.FromResult(weeklyExpenses.AsEnumerable());
        }

        private int CalculateWeekNumber(DateTime date)
        {
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        Task<IEnumerable<MonthlyExpense>> IExpenseCalculatorService.CalculateMonthlyExpenseAsync(IEnumerable<Expense> expenses)
        {
            var monthlyExpenses = expenses
                .GroupBy(e => e.Date.Month)
                .Select(g => new MonthlyExpense
                {
                    Month = g.Key,
                    TotalMonthlyExpense = g.Sum(e => e.Price)
                })
                .OrderBy(me => me.Month);

            return Task.FromResult(monthlyExpenses.AsEnumerable());
        }
    }
}
