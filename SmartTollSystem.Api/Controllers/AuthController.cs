using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartTollSystem.Application.Contracts;
using SmartTollSystem.Application.DTOs;

namespace SmartTollSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegisterDTO)
        {
            var result = await _authService.RegisterAsync(userRegisterDTO);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            var result = await _authService.LoginAsync(userLoginDTO);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("userId")]
        public async Task<IActionResult> GetUserIdFromToken(string token)
        {
            var userId = await _authService.GetUserIdFromTokenAsync(token);
            if (userId != null)
            {
                return Ok(userId);
            }
            return BadRequest("Invalid token");
        }
        [HttpGet("role")]
        public async Task<IActionResult> GetRoleFromToken(string token)
        {
            var role = await _authService.GetRoleFromTokenAsync(token);
            if (role != null)
            {
                return Ok(role);
            }
            return BadRequest("Invalid token");
        }
        [HttpGet("expiration")]
        public async Task<IActionResult> GetExpirationDateFromToken(string token)
        {
            var expirationDate = await _authService.GetExpirationDateFromTokenAsync(token);
            if (expirationDate != null)
            {
                return Ok(expirationDate);
            }
            return BadRequest("Invalid token");
        }

    }
}
