namespace BudgetAPI.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? BudgetId { get; set; }
        public User User { get; set; } = null!;
        public Budget? Budget { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; } = string.Empty;
        
    }
}
