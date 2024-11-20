using BudgetAPI.Data;
using BudgetAPI.Models;
using BudgetAPI.Mapping;
using BudgetAPI.DTOs;
using AutoMapper;
using BCrypt.Net; // Corrected import for BCrypt
using Microsoft.EntityFrameworkCore;

namespace BudgetAPI.Services
{
    public class UserService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserService(AppDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        // Register a User
        public async Task<UserResponseDto> RegisterAsync(UserRegisterDto request)
        {
            // Check if the user already exists by email
            if (await _dbContext.Users.AnyAsync(u => u.Email == request.Email))
                throw new ArgumentException("Email already in use");

            // Map the DTO to the user model
            var newUser = _mapper.Map<User>(request);

            // Hash the Password using BCrypt
            newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Add User to the database 
            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            // Map the saved user back to a response DTO
            return _mapper.Map<UserResponseDto>(newUser);
        }

        // User Login
        public async Task<UserLoginResponseDto> LoginAsync(UserLoginDto request)
        {
            // Find the user by email
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            // Handle invalid credentials
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                // Instead of throwing directly, you could log this event if desired
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            // Here, you would generate a JWT token if using authentication
            var token = "SampleTokenForNow";  // Replace with real token generation logic

            // Return the user and token
            return new UserLoginResponseDto
            {
                Token = token,
                UserId = user.Id,
                Username = user.Username
            };
        }

        // Update a User
        public async Task<UserResponseDto> UpdateUserAsync(int userId, UserUpdateDto request)
        {
            // Get the user by ID
            var user = await _dbContext.Users.FindAsync(userId);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            // Update fields
            user.Username = request.Username;
            user.Email = request.Email;

            // Save changes
            await _dbContext.SaveChangesAsync();

            // Return updated user info as a response DTO
            return _mapper.Map<UserResponseDto>(user);
        }

        // Get User by ID
        public async Task<UserResponseDto> GetByIdAsync(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            return _mapper.Map<UserResponseDto>(user);
        }
    }
}
