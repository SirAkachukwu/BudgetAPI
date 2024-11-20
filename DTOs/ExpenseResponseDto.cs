namespace BudgetAPI.DTOs
{
    public class ExpenseResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; } = string.Empty;
        
    }
}


// This DTO is used to return information about an expense when queried, so sensitive information (e.g., UserId) is not exposed in API responses