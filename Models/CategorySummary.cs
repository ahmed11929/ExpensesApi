namespace ExpensesApi.Models
{
    public class CategorySummary
    {
        public string? Category { get; set; }
        public decimal Total { get; set; }
        public int Count { get; set; }
    }
}