namespace BudgetAPI.DTOs
{
    public class BudgetTrackingDto
    {
        public decimal TotalSpent { get; set; }
        public decimal RemainingBudget { get; set; }
        public decimal TotalBudgetAmount { get; set; }
        public string? WarningMessage { get; set; }
        public Dictionary<string, decimal>? ExpenseBreakdown { get; set; } // Breakdown of expenses by category
    }
}
