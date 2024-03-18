using DailyExpenseAPI.Models;
using DailyExpenseAPI.Services;
using DailyExpenseAPI.Services.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DailyExpenseAPI.Tests
{
    public class ExpenseCalculatorServiceTests
    {
        private readonly IExpenseCalculatorService _expenseCalculatorService;

        public ExpenseCalculatorServiceTests()
        {
            _expenseCalculatorService = new ExpenseCalculatorService();
        }

        [Fact]
        public async Task CalculateDailyExpenseAsync_ReturnsCorrectDailyExpenses()
        {
            // Arrange
            var expenses = new List<Expense>
            {
                new Expense { Date = DateTime.Today, Price = 10 },
                new Expense { Date = DateTime.Today, Price = 20 },
                new Expense { Date = DateTime.Today.AddDays(-1), Price = 15 }
            };

            // Act
            var result = await _expenseCalculatorService.CalculateDailyExpenseAsync(expenses);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count()); // Expecting two daily expense records for today and one for yesterday
            // Add more assertions as needed
        }

        [Fact]
        public async Task CalculateWeeklyExpenseAsync_ReturnsCorrectWeeklyExpenses()
        {
            // Arrange
            var expenses = new List<Expense>
            {
               new Expense { Date = DateTime.Today, Price = 10 },
               new Expense { Date = DateTime.Today.AddDays(-5), Price = 20 },
               new Expense { Date = DateTime.Today.AddDays(-8), Price = 15 }
            };

            // Act
            var result = await _expenseCalculatorService.CalculateWeeklyExpenseAsync(expenses);

            // Assert
            Assert.NotNull(result);

            // Check the total weekly expense for each week
            Assert.Collection(result,
                weeklyExpense =>
                {
                    Assert.Equal(7, weeklyExpense.WeekNumber); // Week number for today's date
                    Assert.Equal(10, weeklyExpense.TotalWeeklyExpense); // Total expense for the current week
                },
                weeklyExpense =>
                {
                    Assert.Equal(6, weeklyExpense.WeekNumber); // Week number for the date 5 days ago
                    Assert.Equal(20, weeklyExpense.TotalWeeklyExpense); // Total expense for the week 5 days ago
                },
                weeklyExpense =>
                {
                    Assert.Equal(5, weeklyExpense.WeekNumber); // Week number for the date 8 days ago
                    Assert.Equal(15, weeklyExpense.TotalWeeklyExpense); // Total expense for the week 8 days ago
                }
            );
        }


    }
}
