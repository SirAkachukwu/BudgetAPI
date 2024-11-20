using BudgetAPI.DTOs;
using BudgetAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BudgetAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        // 1. Register a new user
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
        {
            var result = await _userService.RegisterAsync(userRegisterDto);
            return CreatedAtAction(nameof(GetUserById), new { id = result.Id }, result);
        }

        // 2. Login user
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            try
            {
                var loginResult = await _userService.LoginAsync(userLoginDto);
                return Ok(loginResult);  // Return 200 OK with the login result
            }
            catch (UnauthorizedAccessException ex)
            {
                // Return a 401 Unauthorized status with the error message
                return Unauthorized(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, "An internal server error occurred.");
            }
        }


        // 3. Update user details
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserUpdateDto userUpdateDto)
        {
            await _userService.UpdateUserAsync(id, userUpdateDto);
            return NoContent();
        }

        // 4. Get user by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var userResponse = await _userService.GetByIdAsync(id);

            if (userResponse == null)
            {
                return NotFound(); // Return 404 if user is not found
            }

            return Ok(userResponse);
        }

    }
}
