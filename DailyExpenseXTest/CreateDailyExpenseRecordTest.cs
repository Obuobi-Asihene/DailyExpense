using DailyExpenseAPI.Data;
using DailyExpenseAPI.Models;
using DailyExpenseAPI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DailyExpenseXTest
{
    public class CreateDailyExpenseRecordsTests
    {
        [Fact]
        public async Task CreateDailyExpenseRecordsAsync_CreatesNewTableAndMovesExpenses()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DailyExpenseDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new DailyExpenseDbContext(options))
            {
                // Seed some test expenses for today
                DateTime today = DateTime.Today;
                context.Expenses.AddRange(new[]
                {
                    new Expense { ItemName = "Shoe", Price = 10, Date = today },
                    new Expense { ItemName = "Book", Price = 20, Date = today }
                });
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new DailyExpenseDbContext(options))
            {
                var service = new ExpenseService(context);
               // await service.CreateDailyExpenseRecordsAsync();
            }

            // Assert
            using (var context = new DailyExpenseDbContext(options))
            {
                DateTime today = DateTime.Today;

                // Check if the new table was created
                var tableExists = await context.Database.CanConnectAsync();
                Assert.True(tableExists);

                // Check if expenses were moved to the new table
                string tableName = $"Expenses_{today:yyyyMMdd}";
                var expensesInNewTable = await context.Expenses.Where(e => e.Date.Date == today).ToListAsync();
                Assert.Equal(2, expensesInNewTable.Count); // Assuming 2 expenses were moved to the new table

                // Check if expenses were removed from the main table
                var expensesInMainTable = await context.Expenses.ToListAsync();
                Assert.Empty(expensesInMainTable); // Expecting no expenses in the main table
            }

        }
    }
}
