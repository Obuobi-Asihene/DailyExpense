using DailyExpenseAPI.Models;

namespace DailyExpenseAPI.Services.Interface
{
    public interface IExpenseService
    {
        Task<IEnumerable<Expense>> GetAllExpnsesAsync();
        Task<Expense> GetExpenseByIdAsync(int id);
        Task AddExpenseAsync(Expense expense);
        Task DeleteExpenseAsync(int id);
        Task UpdateExpenseAsync(Expense expense);
        Task CreateDailyExpenseRecordsAsync();
    }
}
