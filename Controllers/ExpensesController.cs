using ExpensesApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpensesController : ControllerBase
    {
        private readonly SpendSmartDbContext _context;

        public ExpensesController(SpendSmartDbContext context)
        {
            _context = context;
        }

        // GET api/expenses
        // GET api/expenses?category=Food&from=2026-10-01&to=2026-10-31
        [HttpGet]
        public IActionResult GetAll(string? category, DateTime? from, DateTime? to)
        {
            var query = _context.Expenses.AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(e => e.Category == category);

            if (from.HasValue)
                query = query.Where(e => e.Date >= from.Value);

            if (to.HasValue)
                query = query.Where(e => e.Date <= to.Value);

            return Ok(query.OrderByDescending(e => e.Date).ToList());
        }

        // GET api/expenses/summary
        [HttpGet("summary")]
        public IActionResult Summary()
        {
            var result = _context.Expenses
                .GroupBy(e => e.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    Total = g.Sum(e => e.Value),
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Total)
                .ToList();

            return Ok(result);
        }

        // GET api/expenses/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var expense = _context.Expenses.Find(id);
            if (expense == null) return NotFound();
            return Ok(expense);
        }

        // POST api/expenses
        [HttpPost]
        public IActionResult Create(Expense expense)
        {
            _context.Expenses.Add(expense);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = expense.Id }, expense);
        }

        // PUT api/expenses/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, Expense expense)
        {
            if (id != expense.Id) return BadRequest("Id in URL and body must match.");

            var existing = _context.Expenses.Find(id);
            if (existing == null) return NotFound();

            _context.Entry(existing).CurrentValues.SetValues(expense);
            _context.SaveChanges();
            return NoContent();
        }

        // DELETE api/expenses/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var expense = _context.Expenses.Find(id);
            if (expense == null) return NotFound();

            _context.Expenses.Remove(expense);
            _context.SaveChanges();
            return NoContent();
        }
    }
}