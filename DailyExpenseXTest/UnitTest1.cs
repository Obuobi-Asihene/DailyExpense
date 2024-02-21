using DailyExpenseAPI.Data;
using DailyExpenseAPI.Models;
using DailyExpenseAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace DailyExpenseXTest
{
    public class ExpenseServiceTests
    {
        [Fact]
        public async Task CalculateDailyExpenseAsync_ReturnsCorrectDailyExpense()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DailyExpenseDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new DailyExpenseDbContext(options))
            {
                // Seed some test expenses
                context.Expenses.AddRange(new List<Expense>
                {
                    new Expense {ItemName = "Shoe", Price = 10, Date = new DateTime(2024, 2, 17) },
                    new Expense {ItemName = "book", Price = 20, Date = new DateTime(2024, 2, 17) },
                    new Expense {ItemName = "soap", Price = 30, Date = new DateTime(2024, 2, 18) },
                    new Expense {ItemName = "bag", Price = 40, Date = new DateTime(2024, 2, 18) }
                });
                await context.SaveChangesAsync();
            }

            using (var context = new DailyExpenseDbContext(options))
            {
                var service = new ExpenseService(context);

                // Act
                var result = await service.CalculateDailyExpenseAsync();

                // Assert
                Assert.NotNull(result);

                var dailyExpense1 = result.FirstOrDefault(de => de.Date == new DateTime(2024, 2, 17));
                Assert.NotNull(dailyExpense1);
                Assert.Equal(30, dailyExpense1.TotalExpense); // 10 + 20 = 30

                var dailyExpense2 = result.FirstOrDefault(de => de.Date == new DateTime(2024, 2, 18));
                Assert.NotNull(dailyExpense2);
                Assert.Equal(70, dailyExpense2.TotalExpense); // 30 + 40 = 70
            }
        }
    }
}
