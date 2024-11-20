namespace BudgetAPI.DTOs
{
    public class ExpenseMonthlyReportDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalExpenses { get; set; }
        public List<ExpenseCategorySummaryDto> CategorySummaries { get; set; }

        public ExpenseMonthlyReportDto()
        {
            CategorySummaries = new List<ExpenseCategorySummaryDto>();
        }
    }
}
