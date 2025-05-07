using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartTollSystem.Application.Contracts;
using SmartTollSystem.Application.DTOs;
using System.Security.Claims;

namespace SmartTollSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }
        /// <summary>
        /// Get wallet balance
        /// </summary>
        /// <returns></returns>
        // 🔒 User: Get wallet balance
        [HttpGet("balance")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetWalletBalance()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out var userGuid))
                return Unauthorized();
            var balance = await _walletService.GetBalanceAsync(userGuid);
            return Ok(new { Balance = balance });
        }
        // 🔒 User: Add funds to wallet
        /// <summary>
        /// Add funds to wallet
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("add-funds")]
        [Authorize]
        public async Task<IActionResult> AddFunds([FromBody] TopUpDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out var userGuid))
                return Unauthorized();
            var result = await _walletService.TopUpBalanceAsync(userGuid, dto.Amount);
            return Ok(result);
        }
    }
}
