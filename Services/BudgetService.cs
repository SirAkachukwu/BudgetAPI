using AutoMapper;
using BudgetAPI.Data;
using BudgetAPI.DTOs;
using BudgetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetAPI.Services
{
    public class BudgetService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public BudgetService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // Add a Budget
        public async Task<BudgetResponseDto> AddBudgetAsync(int userId, BudgetCreateDto budgetCreateDto)
        {
            ValidateBudgetDates(budgetCreateDto.StartDate, budgetCreateDto.EndDate);

            var budget = _mapper.Map<Budget>(budgetCreateDto);
            budget.UserId = userId; // Associate budget with the user

            _dbContext.Budgets.Add(budget);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<BudgetResponseDto>(budget);
        }

        // Update a Budget
        public async Task<BudgetResponseDto> UpdateBudgetAsync(int userId, int budgetId, BudgetUpdateDto budgetUpdateDto)
        {
            ValidateBudgetDates(budgetUpdateDto.StartDate, budgetUpdateDto.EndDate);

            var budget = await _dbContext.Budgets
                .FirstOrDefaultAsync(b => b.Id == budgetId && b.UserId == userId);

            if (budget == null)
            {
                throw new Exception("Budget not found");
            }

            _mapper.Map(budgetUpdateDto, budget);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<BudgetResponseDto>(budget);
        }

        // Get All Budgets
        public async Task<List<BudgetResponseDto>> GetAllBudgetsAsync(int userId)
        {
            var budgets = await _dbContext.Budgets
                .Where(e => e.UserId == userId)
                .ToListAsync();

            return _mapper.Map<List<BudgetResponseDto>>(budgets);
        }

        // Get Budget by Id
        public async Task<BudgetResponseDto> GetBudgetByIdAsync(int userId, int budgetId)
        {
            // Find the budget
            var budget = await _dbContext.Budgets.FirstOrDefaultAsync(e => e.Id == budgetId && e.UserId == userId);

            if (budget == null)
            {
                throw new Exception("Budget not found or does not belong to the user");
            }

            return _mapper.Map<BudgetResponseDto>(budget);
        }

        // Get Current Budget
        public async Task<BudgetResponseDto> GetCurrentBudgetAsync(int userId)
        {
            var currentBudget = await _dbContext.Budgets
                .OrderByDescending(b => b.StartDate)
                .FirstOrDefaultAsync(b => b.UserId == userId && b.EndDate > DateTime.Now);

            if (currentBudget == null)
            {
                throw new Exception("No active budget found");
            }

            return _mapper.Map<BudgetResponseDto>(currentBudget);
        }

        // Delete a Budget
        public async Task<bool> DeleteBudgetAsync(int userId, int budgetId)
        {
            // Retrieve the budget
            var budget = await _dbContext.Budgets
                .FirstOrDefaultAsync(b => b.Id == budgetId && b.UserId == userId);

            if (budget == null)
            {
                throw new Exception("Budget not found");
            }

            // Delete all expenses associated with this budget
            var associatedExpenses = await _dbContext.Expenses
                .Where(e => e.BudgetId == budgetId)
                .ToListAsync();

            _dbContext.Expenses.RemoveRange(associatedExpenses);
            _dbContext.Budgets.Remove(budget);

            await _dbContext.SaveChangesAsync();

            return true;
        }



        // Delete All Budget by User
        public async Task<bool> DeleteAllBudgetsByUserAsync(int userId)
        {
            var userBudgets = await _dbContext.Budgets.Where(e => e.UserId == userId)
                                                      .ToListAsync();
            if (userBudgets.Count == 0)
            {
                throw new Exception("No budgets found for the user.");
            }

            // Remove the budgets
            _dbContext.Budgets.RemoveRange(userBudgets);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        // Track Spending and Warn When Close to Budget by ID
        public async Task<BudgetTrackingDto> TrackAndWarnBudgetAsync(int userId, int budgetId)
        {
            var currentBudget = await _dbContext.Budgets.FirstOrDefaultAsync(b => b.UserId == userId && b.Id == budgetId);
            if (currentBudget == null)
            {
                throw new Exception("Budget not found");
            }

            // Retrieve expenses and sum in memory
            var expenses = await _dbContext.Expenses
                .Where(e => e.UserId == userId && e.Date >= currentBudget.StartDate && e.Date <= currentBudget.EndDate)
                .ToListAsync(); // Load expenses into memory

            var totalExpenses = expenses.Sum(e => e.Amount); // Sum in memory
            var remainingBudget = currentBudget.Amount - totalExpenses; // Remaining budget

            // Create an expense breakdown by category
            var expenseBreakdown = expenses
                .GroupBy(e => e.Category)
                .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));

            // Check if the user has spent 90% or more of their budget
            var warningThreshold = 0.9m * currentBudget.Amount;

            var budgetTrackingDto = new BudgetTrackingDto
            {
                TotalSpent = totalExpenses,
                RemainingBudget = remainingBudget,
                TotalBudgetAmount = currentBudget.Amount,
                ExpenseBreakdown = expenseBreakdown
            };

            if (totalExpenses > warningThreshold)
            {
                budgetTrackingDto.WarningMessage = "Warning: You are close to exceeding your budget!";
            }

            return budgetTrackingDto;
        }



        // Validate budget dates
        private void ValidateBudgetDates(DateTime startDate, DateTime endDate)
        {
            if (endDate.Date <= startDate.Date)
            {
                throw new Exception("EndDate must be greater than StartDate.");
            }
        }


    }
}
