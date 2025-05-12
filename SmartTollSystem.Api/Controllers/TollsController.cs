using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartTollSystem.Application.DTOs;
using SmartTollSystem.Application.Services;
using SmartTollSystem.Domain.DTOs;
using SmartTollSystem.Domain.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartTollSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TollsController : ControllerBase
    {
        private readonly ITollService _tollService;

        public TollsController(ITollService tollService)
        {
            _tollService = tollService;
        }

        /// <summary>
        /// Get all toll transactions
        /// </summary>
        /// <returns></returns>
        // 🔓 Admin: Get all toll transactions
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllTolls()
        {
            var tolls = await _tollService.GetAllTollsAsync();
            return Ok(tolls);
        }

        // 🔒 Logged-in vehicle owner: Get my toll history
        /// <summary>
        /// Get my toll history
        /// </summary>
        /// <returns></returns>
        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyTolls()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out var userGuid))
                return Unauthorized();

            var myTolls = await _tollService.GetTollHistoryAsync(userGuid);
            return Ok(myTolls);
        }

        // 🔓 Radar or user: Pay toll using plate + radar
        /// <summary>
        /// Pay toll using plate number
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("pay")]
       
        public async Task<IActionResult> PayToll([FromBody] LicensePlateDto dto)
        {
            var result = await _tollService.ProcessTollAsyncV1(dto);
            return Ok(result);
        }

        // 🔓 Admin: Get toll by ID
        /// <summary>
        /// Get a toll transaction by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetTollById(Guid id)
        {
            var toll = await _tollService.GetTollByIdAsync(id);
            if (toll == null) return NotFound();
            return Ok(toll);
        }
    }
}
