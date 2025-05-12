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
        [Authorize]
        public async Task<IActionResult> GetWalletBalance()
        {
            // Extract user ID from the claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out var userGuid))
                return Unauthorized("Invalid user ID."); // Return 401 Unauthorized if user ID is invalid

            try
            {
                // Get the balance from the service
                var balance = await _walletService.GetBalanceAsync(userGuid);

                // If balance is 0, we can return a response, but 200 OK is still appropriate
                if (balance == 0)
                {
                    return Ok(new { Balance = 0, Message = "Your balance is currently empty." });
                }

                return Ok(new { Balance = balance }); // Return the balance in the response
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors during balance retrieval
                return StatusCode(500, new { Message = "An error occurred while retrieving the balance.", Error = ex.Message });
            }
        }
        /// <summary>
        /// Add funds to the wallet
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>

        [HttpPost("add-funds")]
        [Authorize]
        public async Task<IActionResult> AddFunds([FromBody] TopUpDto dto)
        {
            // Extract user ID from the claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out var userGuid))
                return Unauthorized("Invalid user ID."); // Return 401 Unauthorized if user ID is invalid

            // Check that the requested top-up amount is positive
            if (dto.Amount <= 0)
            {
                return BadRequest("Top-up amount must be greater than zero."); // Return 400 if the amount is invalid
            }

            try
            {
                // Call the service to add funds to the wallet
                var result = await _walletService.TopUpBalanceAsync(userGuid, dto.Amount);

                if (result)
                {
                    return Ok(new { Message = "Funds successfully added to your wallet." });
                }
                else
                {
                    return BadRequest("Failed to add funds. Please try again."); // Return 400 if the top-up fails
                }
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors during the top-up process
                return StatusCode(500, new { Message = "An error occurred while adding funds.", Error = ex.Message });
            }
        }

    }
}
