using DailyExpenseAPI.Services.Interface;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace DailyExpenseAPI.Services
{
    public class DailyRecordService
    {
        private readonly IExpenseService _expenseService;
        public DailyRecordService(IConfiguration configuration, ExpenseService expenseService)
        {
            _expenseService = expenseService;

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            GlobalConfiguration.Configuration.UseSqlServerStorage(connectionString, new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true

            });

            //schedule recurring job
            RecurringJob.AddOrUpdate("CreateDailyExpenseRecords", () => _expenseService.CreateDailyExpenseRecordsAsync(), Cron.Daily(0, 0));
        }
    }
}
