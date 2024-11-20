namespace BudgetAPI.DTOs
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}


// This DTO is used to return information about the user after registration or login, without sensitive information like passwords.

