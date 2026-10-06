using ExpensesApi.Models;
using ExpensesApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BudgetsController : ControllerBase
    {
        private readonly IBudgetService _service;

        public BudgetsController(IBudgetService service)
        {
            _service = service;
        }

        // GET api/budgets
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        // GET api/budgets/status            (current month)
        // GET api/budgets/status?year=2026&month=10
        [HttpGet("status")]
        public async Task<IActionResult> Status(int? year, int? month)
        {
            var today = DateTime.Today;
            var y = year ?? today.Year;
            var m = month ?? today.Month;

            if (m < 1 || m > 12 || y < 2000 || y > 2100)
                return BadRequest("Invalid year or month.");

            return Ok(await _service.GetStatusAsync(y, m));
        }

        // POST api/budgets   (creates or updates the budget of a category)
        [HttpPost]
        public async Task<IActionResult> Set(Budget budget)
        {
            var saved = await _service.SetBudgetAsync(budget.Category, budget.MonthlyLimit);
            return Ok(saved);
        }

        // DELETE api/budgets/3
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}