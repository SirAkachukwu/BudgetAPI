using BudgetAPI.DTOs;
using BudgetAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BudgetAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpensesController : ControllerBase
    {
        private readonly ExpenseService _expenseService;

        public ExpensesController(ExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        // 1. Add an Expense
        [HttpPost("user/{userId}/add-expense")]
        public async Task<IActionResult> AddExpense(int userId, [FromBody] ExpenseCreateDto expenseCreateDto)
        {
            var expense = await _expenseService.AddExpenseAsync(userId, expenseCreateDto);
            return CreatedAtAction(nameof(GetExpenseById), new { userId = userId, expenseId = expense.Id }, expense);
        }

        // Update an Expense endpoint 
        [HttpPut("user/{userId}/expense/{expenseId}/update-expense")]
        public async Task<IActionResult> UpdateExpense(int userId, int expenseId, [FromBody] ExpenseUpdateDto expenseUpdateDto)
        {
            try
            {
                var updatedExpense = await _expenseService.UpdateExpenseAsync(userId, expenseId, expenseUpdateDto);
                return Ok(updatedExpense);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // handle any other unexpected exceptions
                return StatusCode(500, new { message = "An error occurred while updating the expense." });
            }
        }


        // 3. Get All Expenses
        [HttpGet("user/{userId}/all-expenses")]
        public async Task<IActionResult> GetAllExpenses(int userId)
        {
            var expenses = await _expenseService.GetAllExpensesAsync(userId);
            return Ok(expenses);
        }

        // 4. Get Expense by Id
        [HttpGet("user/{userId}/expense/{expenseId}")]
        public async Task<IActionResult> GetExpenseById(int userId, int expenseId)
        {
            var expense = await _expenseService.GetExpenseByIdAsync(userId, expenseId);
            
            return Ok(expense);
        }

        // Get Expenses by BudgetID
        [HttpGet("user/{userId}/expenses/budget/{budgetId}")]
        public async Task<IActionResult> GetExpensesByBudgetId(int userId, int budgetId)
        {
            var expenses = await _expenseService.GetExpensesByBudgetIdAsync(userId, budgetId);

            if (expenses.Count == 0)
            {
                return NotFound("No expenses found for this budget.");
            }

            return Ok(expenses);
        }


        // 5. Filter Expenses
        [HttpGet("user/{userId}/filter")]
        public async Task<IActionResult> FilterExpenses(int userId, DateTime? startDate, DateTime? endDate, string? category)
        {
            var expenses = await _expenseService.FilterExpensesAsync(userId, startDate, endDate, category);
            return Ok(expenses);
        }

        // 6. Generate Monthly Report
        [HttpGet("user/{userId}/monthly-report")]
        public async Task<IActionResult> GenerateMonthlyReport(int userId, int year, int month)
        {
            var report = await _expenseService.GenerateMonthlyReportAsync(userId, year, month);
            return Ok(report);
        }

        // 7. Delete an Expense
        [HttpDelete("user/{userId}/expense/{expenseId}")]
        public async Task<IActionResult> DeleteExpense(int userId, int expenseId)
        {
            await _expenseService.DeleteExpenseAsync(userId, expenseId);
            return NoContent();
        }

        // 8. Delete all Expenses by User
        [HttpDelete("user/{userId}/all-expenses")]
        public async Task<IActionResult> DeleteAllExpensesByUser(int userId)
        {
            await _expenseService.DeleteAllExpensesByUserAsync(userId);
            return NoContent();
        }


    }
}
