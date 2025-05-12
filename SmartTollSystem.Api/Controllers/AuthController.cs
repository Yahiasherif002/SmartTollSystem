using Microsoft.AspNetCore.Authorization;
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
        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="userRegisterDTO"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Login a user
        /// </summary>
        /// <param name="userLoginDTO"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Get user ID from token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Get user role from token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Get expiration date from token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
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

        [Authorize]
        [HttpGet("currentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var currentUser = await _authService.GetCurrentUserAsync(token);
            if (currentUser != null)
            {
                return Ok(currentUser);
            }
            return BadRequest("Invalid token");
        }

    }
}
