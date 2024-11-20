using BudgetAPI.DTOs;
using BudgetAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BudgetAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BudgetsController : ControllerBase
    {
        private readonly BudgetService _budgetService;

        public BudgetsController(BudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        // 1. Add a Budget
        [HttpPost("user/{userId}/add-budget")]
        public async Task<IActionResult> AddBudget(int userId, [FromBody] BudgetCreateDto budgetCreateDto)
        {
            var budget = await _budgetService.AddBudgetAsync(userId, budgetCreateDto);
            return CreatedAtAction(nameof(GetCurrentBudget), new { userId = userId, budgetId = budget.Id }, budget);
        }

        // 2. Update a Budget
        [HttpPut("user/{userId}/budget/{budgetId}/update-budget")]
        public async Task<IActionResult> UpdateBudget(int userId, int budgetId, [FromBody] BudgetUpdateDto budgetUpdateDto)
        {
            var updatedBudget = await _budgetService.UpdateBudgetAsync(userId, budgetId, budgetUpdateDto);
            return Ok(updatedBudget);
        }

        // 3. Get All Budgets
        [HttpGet("user/{userId}/all-budgets")]
        public async Task<IActionResult> GetAllBudgets(int userId)
        {
            var expenses = await _budgetService.GetAllBudgetsAsync(userId);
            return Ok(expenses);
        }

        // 4. Get Budget by Id
        [HttpGet("user/{userId}/budget/{budgetId}")]
        public async Task<IActionResult> GetBudgetById(int userId, int budgetId)
        {
            var budgets = await _budgetService.GetBudgetByIdAsync(userId, budgetId);
            return Ok(budgets);
        }

        // 5. Get Current Budget by ID
        [HttpGet("{userId}/current-budget")]
        public async Task<IActionResult> GetCurrentBudget(int userId)
        {
            var budget = await _budgetService.GetCurrentBudgetAsync(userId);
            return Ok(budget);
        }

        // 6. Track Spending and Warn When Close to Budget by ID
        [HttpGet("{userId}/budgets/{budgetId}/track-and-warn")]
        public async Task<IActionResult> TrackAndWarnBudget(int userId, int budgetId)
        {
            var trackingData = await _budgetService.TrackAndWarnBudgetAsync(userId, budgetId);
            return Ok(trackingData);
        }

        // 7. Delete a Budget
        [HttpDelete("user/{userId}/budget/{budgetId}/delete-budget")]
        public async Task<IActionResult> DeleteBudget(int userId, int budgetId)
        {
            await _budgetService.DeleteBudgetAsync(userId, budgetId);
            return NoContent();
        }

        // 8. Delete all Budgets by User
        [HttpDelete("user/{userId}/all-budgets")]
        public async Task<IActionResult> DeleteAllBudgetsByUser(int userId)
        {
            await _budgetService.DeleteAllBudgetsByUserAsync(userId);
            return NoContent();

        }
    }
}
