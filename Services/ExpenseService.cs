using AutoMapper;
using BudgetAPI.Data;
using BudgetAPI.DTOs;
using BudgetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetAPI.Services
{
    public class ExpenseService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public ExpenseService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // Add an Expense
        public async Task<ExpenseResponseDto> AddExpenseAsync(int userId, ExpenseCreateDto expenseDto)
        {
            // Map the DTO to the Expense entity/model
            var expense = _mapper.Map<Expense>(expenseDto);
            expense.UserId = userId; // Associate the expense with the user

            // Add the new expense to the context
            _dbContext.Expenses.Add(expense);
            await _dbContext.SaveChangesAsync();

            // Map the entity back to a response DTO
            return _mapper.Map<ExpenseResponseDto>(expense);
        }

        //Update an Expense
        public async Task<ExpenseResponseDto> UpdateExpenseAsync(int userId, int expenseId, ExpenseUpdateDto expenseDto)
        {
            // Find the expense in the database
            var expense = await _dbContext.Expenses.FirstOrDefaultAsync(e => e.Id == expenseId && e.UserId == userId);

            if (expense == null)
            {
                throw new KeyNotFoundException("Expense not found or does not belong to the user");
            }

            // Map the updates from the DTO to the existing entity
            _mapper.Map(expenseDto, expense);

            // Update the expense in the context
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ExpenseResponseDto>(expense);
        }

        // Get All Expenses
        public async Task<List<ExpenseResponseDto>> GetAllExpensesAsync(int userId)
        {
            var expenses = await _dbContext.Expenses
                .Where(e => e.UserId == userId)
                .ToListAsync();

            return _mapper.Map<List<ExpenseResponseDto>>(expenses);
        }

        // Get Expenses by ID
        public async Task<ExpenseResponseDto> GetExpenseByIdAsync(int userId, int expenseId)
        {
            // Find the expense
            var expense = await _dbContext.Expenses.FirstOrDefaultAsync(e => e.Id == expenseId && e.UserId == userId);

            if (expense == null)
            {
                throw new KeyNotFoundException("Expense not found or does not belong to the user");
            }

            return _mapper.Map<ExpenseResponseDto>(expense);
        }

        // Get Expenses by BudgetID
        public async Task<List<ExpenseResponseDto>> GetExpensesByBudgetIdAsync(int userId, int budgetId)
        {
            var expenses = await _dbContext.Expenses
                .Where(e => e.UserId == userId && e.BudgetId == budgetId)
                .ToListAsync();

            return _mapper.Map<List<ExpenseResponseDto>>(expenses);
        }



        // Delete an Expense
        public async Task<bool> DeleteExpenseAsync(int userId, int expenseId)
        {
            // Find the expense
            var expense = await _dbContext.Expenses.FirstOrDefaultAsync(e => e.Id == expenseId && e.UserId == userId);

            if (expense == null)
            {
                throw new KeyNotFoundException("Expense not found or does not belong to the user");
            }

            // Remove the expense
            _dbContext.Expenses.Remove(expense);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        // Delete All Expenses by User
        public async Task<bool> DeleteAllExpensesByUserAsync(int userId)
        {
            var userExpenses = await _dbContext.Expenses.Where(e => e.UserId == userId)
                                                      .ToListAsync();
            if (userExpenses.Count == 0)
            {
                throw new KeyNotFoundException("No expenses found for the user.");
            }

            // Remove the expenses
            _dbContext.Expenses.RemoveRange(userExpenses);
            await _dbContext.SaveChangesAsync();

            return true;
        }


        // Filter 
        public async Task<List<ExpenseResponseDto>> FilterExpensesAsync(int userId, DateTime? startDate, DateTime? endDate, string? category)
        {
            var query = _dbContext.Expenses.AsQueryable();

            // Filter by UserId
            query = query.Where(e => e.UserId == userId);

            // Combine date filtering
            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(e => e.Date >= startDate.Value && e.Date <= endDate.Value);
            }
            else
            {
                if (startDate.HasValue)
                {
                    query = query.Where(e => e.Date >= startDate.Value);
                }

                if (endDate.HasValue)
                {
                    query = query.Where(e => e.Date <= endDate.Value);
                }
            }

            // Filter by category if provided
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(e => e.Category == category);
            }

            var filteredExpenses = await query.ToListAsync();

            return _mapper.Map<List<ExpenseResponseDto>>(filteredExpenses);
        }


        // Generate Monthly Reports
        public async Task<ExpenseMonthlyReportDto> GenerateMonthlyReportAsync(int userId, int year, int month)
        {
            var expenses = await _dbContext.Expenses
                .Where(e => e.UserId == userId && e.Date.Year == year && e.Date.Month == month)
                .ToListAsync();

            // Calculate total expenses for the specified month
            var totalExpenses = expenses.Sum(e => e.Amount);
            // Group Expenses by category and create summaries
            var categorySummaries = expenses
                .GroupBy(e => e.Category)
                .Select(g => new ExpenseCategorySummaryDto
                {
                    Category = g.Key,
                    TotalAmount = g.Sum(e => e.Amount)
                })
                .ToList();
            // Return the monthly report DTO
            return new ExpenseMonthlyReportDto
            {
                Year = year,
                Month = month,
                TotalExpenses = totalExpenses,
                CategorySummaries = categorySummaries
            };
        }

    }

}
