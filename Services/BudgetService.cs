using ExpensesApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpensesApi.Services
{
    public class BudgetService : IBudgetService
    {
        private const decimal WarningPercent = 80;   // change to alert earlier or later

        private readonly SpendSmartDbContext _context;

        public BudgetService(SpendSmartDbContext context)
        {
            _context = context;
        }

        public async Task<List<Budget>> GetAllAsync()
        {
            return await _context.Budgets.OrderBy(b => b.Category).ToListAsync();
        }

        // Creates the budget, or updates it if the category already has one
        public async Task<Budget> SetBudgetAsync(string category, decimal monthlyLimit)
        {
            category = category.Trim();

            var existing = await _context.Budgets.FirstOrDefaultAsync(b => b.Category == category);
            if (existing == null)
            {
                existing = new Budget { Category = category, MonthlyLimit = monthlyLimit };
                _context.Budgets.Add(existing);
            }
            else
            {
                existing.MonthlyLimit = monthlyLimit;
            }

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Budgets.FindAsync(id);
            if (existing == null) return false;

            _context.Budgets.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<BudgetStatus>> GetStatusAsync(int year, int month)
        {
            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1);

            // total spent per category in that month (done by SQL Server)
            var totals = await _context.Expenses
                .Where(e => e.Date >= start && e.Date < end && e.Category != null)
                .GroupBy(e => e.Category!)
                .Select(g => new { Category = g.Key, Total = g.Sum(e => e.Value) })
                .ToListAsync();

            var spentByCategory = totals.ToDictionary(
                x => x.Category, x => x.Total, StringComparer.OrdinalIgnoreCase);

            var budgets = await _context.Budgets.OrderBy(b => b.Category).ToListAsync();

            return budgets.Select(b =>
            {
                spentByCategory.TryGetValue(b.Category, out var spent);
                var percent = Math.Round(spent / b.MonthlyLimit * 100, 1);

                return new BudgetStatus
                {
                    Id = b.Id,
                    Category = b.Category,
                    Limit = b.MonthlyLimit,
                    Spent = spent,
                    Remaining = b.MonthlyLimit - spent,
                    PercentUsed = percent,
                    Status = spent > b.MonthlyLimit ? "over"
                           : percent >= WarningPercent ? "warning"
                           : "ok"
                };
            }).ToList();
        }
    }
}