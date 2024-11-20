namespace BudgetAPI.DTOs
{
    public class ExpenseCategorySummaryDto
    {
        public string Category { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }

    }
}
