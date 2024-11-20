namespace BudgetAPI.DTOs
{
    public class UserLoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;  // You can add more fields as necessary

    }
}

