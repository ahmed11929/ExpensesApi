namespace ExpensesApi.Models
{
    public class BudgetStatus
    {
        public int Id { get; set; }
        public string Category { get; set; } = string.Empty;
        public decimal Limit { get; set; }
        public decimal Spent { get; set; }
        public decimal Remaining { get; set; }
        public decimal PercentUsed { get; set; }
        public string Status { get; set; } = "ok";   // ok, warning, over
    }
}