namespace BudgetAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public List<Expense> Expenses { get; set; } = new List<Expense>();
        public List<Budget> Budgets { get; set; } = new List<Budget>();
    }
}
