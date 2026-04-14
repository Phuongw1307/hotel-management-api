using Hotel_Management_API.DTOs;
using Hotel_Management_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hotel_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);

            if (result == null)
                return BadRequest(new { message = "Username already exists" });

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);

            if (result == null)
                return Unauthorized(new { message = "Invalid username or password" });

            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            var result = await _authService.RefreshAsync(request.RefreshToken);

            if (result == null)
                return Unauthorized(new { message = "Invalid or expired refresh token" });

            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshRequest request)
        {
            var success = await _authService.LogoutAsync(request.RefreshToken);

            if (!success)
                return BadRequest(new { message = "Refresh token not found" });

            return Ok(new { message = "Logged out successfully" });
        }

        [Authorize]
        [HttpPost("logout-all")]
        public async Task<IActionResult> LogoutAll()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized(new { message = "Invalid user id" });

            var success = await _authService.LogoutAllAsync(userId);

            if (!success)
                return BadRequest(new { message = "User not found" });

            return Ok(new { message = "Logged out from all devices successfully" });
        }
    }
}
