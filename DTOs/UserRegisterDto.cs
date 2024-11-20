namespace BudgetAPI.DTOs
{
    public class UserRegisterDto
    {
        public string Username { get; set; } = string.Empty; // Make sure this property is included
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}


// This DTO handles data for user registration