namespace BudgetAPI.DTOs
{
    public class ExpenseUpdateDto
    {
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        // Make BudgetId optional
        public int? BudgetId { get; set; }

    }
}

// This DTO handles data for updating an existing expense.

