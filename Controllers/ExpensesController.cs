using ExpensesApi.Models;
using ExpensesApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService _service;

        public ExpensesController(IExpenseService service)
        {
            _service = service;
        }

        // GET api/expenses?category=Food&from=2026-10-01&to=2026-10-31
        [HttpGet]
        public async Task<IActionResult> GetAll(string? category, DateTime? from, DateTime? to)
        {
            return Ok(await _service.GetAllAsync(category, from, to));
        }

        // GET api/expenses/summary
        [HttpGet("summary")]
        public async Task<IActionResult> Summary()
        {
            return Ok(await _service.GetSummaryAsync());
        }

        // GET api/expenses/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var expense = await _service.GetByIdAsync(id);
            if (expense == null) return NotFound();
            return Ok(expense);
        }

        // POST api/expenses
        [HttpPost]
        public async Task<IActionResult> Create(Expense expense)
        {
            var created = await _service.CreateAsync(expense);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT api/expenses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Expense expense)
        {
            if (id != expense.Id) return BadRequest("Id in URL and body must match.");

            var updated = await _service.UpdateAsync(id, expense);
            if (!updated) return NotFound();
            return NoContent();
        }

        // DELETE api/expenses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}