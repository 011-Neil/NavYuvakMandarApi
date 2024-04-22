using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using NavYuvakMandarApi.Models;
using NavYuvakMandarApi.Repositories;
using System.Security.Cryptography;

namespace NavYuvakMandarApi.Controllers
{
    [ApiController]
    [Route("api/")]
    [EnableCors]
    public class RegisterUserController : Controller
    {

        private readonly IUserRepository _userRepository;
        private readonly ILogger<RegisterUserController> _logger;


        public RegisterUserController(IUserRepository userRepository, ILogger<RegisterUserController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;

        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {

                var user_list  =  await _userRepository.GetUsers();
                return Ok(user_list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user details");
                return StatusCode(500, "Internal server error");
            }

        }
        [HttpPost("user/register")]
            public async Task<IActionResult> RegisterUser([FromBody] User user)
            {
                try
                {
                    if (await _userRepository.IsUsernameTaken(user.username))
                    {
                        return BadRequest("Username already taken.");
                    }

                    var salt = GenerateSalt();
                    user.salt = Convert.ToBase64String(salt);
                    user.password = HashPassword(user.password, salt);

                    await _userRepository.RegisterUser(user);

                    _logger.LogInformation($"User '{user.username}' registered successfully.");
                    return Ok("Registration successful.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error registering user");
                    return StatusCode(500, "Internal server error");
                }

            
            }
        [HttpPut("user/updatedetails")]
        public async Task<IActionResult> UpdateUserDetails([FromBody] User user)
        {
            try
            {

                string username = user.username;


                await _userRepository.UpdateUserDetails(user);

                _logger.LogInformation($"User '{username}' updated details.");
                return Ok($"User '{username}' updated details.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user details");
                return StatusCode(500, "Internal server error");
            }
        }

        [AllowAnonymous]
        [HttpPost("user/login")]
        public async Task<IActionResult> Login([FromBody] Models.LoginRequest loginRequest)
        {
            var user = await _userRepository.AuthenticateUser(loginRequest.Username, loginRequest.Password);


            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            // Check if session state is available
            if (HttpContext.Session != null)
            {
                // Store user ID in session
                HttpContext.Session.SetInt32("UserId", user.user_id);
            }
            else
            {
                // Handle error when session state is not configured
                return StatusCode(500, "Session state is not configured.");
            }

            return Ok("Login successful.");
        }

        [HttpPost("user/logout")]
        public IActionResult Logout()
        {
            // Clear user session
            if (HttpContext.Session != null)
            {
                // Clear user session
                HttpContext.Session.Clear();
            }
            else
            {
                // Handle error when session state is not configured
                return StatusCode(500, "Session state is not configured.");
            }

            return Ok("Logout successful.");
        }

        [HttpDelete("user/delete")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {

                await _userRepository.DeleteUser(userId);

                _logger.LogInformation($"User with  '{userId}' deleted.");
                return Ok($"User with  '{userId}' deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user details");
                return StatusCode(500, "Internal server error");
            }
        }
        // Hash the password using PBKDF2
        private string HashPassword(string password, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }

        // Generate a salt
        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        private bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);
            string hashedPassword = HashPassword(enteredPassword, saltBytes);

            return storedHash == hashedPassword;
        }

    }
}
