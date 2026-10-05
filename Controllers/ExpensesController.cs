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

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Expenses.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var expense = _context.Expenses.Find(id);
            if (expense == null) return NotFound();
            return Ok(expense);
        }

        [HttpPost]
        public IActionResult Create(Expense expense)
        {
            _context.Expenses.Add(expense);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = expense.Id }, expense);
        }

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