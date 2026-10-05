using ExpensesApi.Models;

namespace ExpensesApi.Services
{
    public interface IExpenseService
    {
        Task<List<Expense>> GetAllAsync(string? category, DateTime? from, DateTime? to);
        Task<List<CategorySummary>> GetSummaryAsync();
        Task<Expense?> GetByIdAsync(int id);
        Task<Expense> CreateAsync(Expense expense);
        Task<bool> UpdateAsync(int id, Expense expense);
        Task<bool> DeleteAsync(int id);
    }
}