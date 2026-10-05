using ExpensesApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpensesApi.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly SpendSmartDbContext _context;

        public ExpenseService(SpendSmartDbContext context)
        {
            _context = context;
        }

        public async Task<List<Expense>> GetAllAsync(string? category, DateTime? from, DateTime? to)
        {
            var query = _context.Expenses.AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(e => e.Category == category);

            if (from.HasValue)
                query = query.Where(e => e.Date >= from.Value);

            if (to.HasValue)
                query = query.Where(e => e.Date <= to.Value);

            return await query.OrderByDescending(e => e.Date).ToListAsync();
        }

        public async Task<List<CategorySummary>> GetSummaryAsync()
        {
            return await _context.Expenses
                .GroupBy(e => e.Category)
                .Select(g => new CategorySummary
                {
                    Category = g.Key,
                    Total = g.Sum(e => e.Value),
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Total)
                .ToListAsync();
        }

        public async Task<Expense?> GetByIdAsync(int id)
        {
            return await _context.Expenses.FindAsync(id);
        }

        public async Task<Expense> CreateAsync(Expense expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return expense;
        }

        public async Task<bool> UpdateAsync(int id, Expense expense)
        {
            var existing = await _context.Expenses.FindAsync(id);
            if (existing == null) return false;

            _context.Entry(existing).CurrentValues.SetValues(expense);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Expenses.FindAsync(id);
            if (existing == null) return false;

            _context.Expenses.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}