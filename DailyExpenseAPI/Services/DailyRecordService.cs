using Hangfire;
using Hangfire.SqlServer;

namespace DailyExpenseAPI.Services
{
    public class DailyRecordService
    {
        public DailyRecordService(IConfiguration configuration)
        {
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
            RecurringJob.AddOrUpdate("CreateDailyExpenseRecords", () => CreateDailyExpenseRecord(), Cron.Daily);
        }

        public void CreateDailyExpenseRecord()
        {
            throw new NotImplementedException();
        }
    }
}
