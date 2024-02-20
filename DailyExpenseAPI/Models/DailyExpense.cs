namespace DailyExpenseAPI.Models
{
    public class DailyExpense
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalExpense { get; set; }
    }
}
