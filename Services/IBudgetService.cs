using ExpensesApi.Models;

namespace ExpensesApi.Services
{
    public interface IBudgetService
    {
        Task<List<Budget>> GetAllAsync();
        Task<Budget> SetBudgetAsync(string category, decimal monthlyLimit);
        Task<bool> DeleteAsync(int id);
        Task<List<BudgetStatus>> GetStatusAsync(int year, int month);
    }
}