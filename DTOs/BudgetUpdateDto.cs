namespace BudgetAPI.DTOs
{
    public class BudgetUpdateDto
    {
        public string? Name { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
