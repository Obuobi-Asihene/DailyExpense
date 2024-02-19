namespace DailyExpenseAPI.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
      //public TimeSpan Time { get; set; }

    }
}
