using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartTollSystem.Application.Contracts;
using SmartTollSystem.Application.DTOs;

namespace SmartTollSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Admin")]

    public class RadarController : ControllerBase
    {
        private readonly IRadarService _radarService;
        public RadarController(IRadarService radarService)
        {
            _radarService = radarService;
        }
        // 🔓 Radar: Get all vehicles
        /// <summary>
        /// Get all radars
        /// </summary>
        /// <returns></returns>
        [HttpGet("radars")]
        public async Task<IActionResult> GetAll()
        {
            var radars = await _radarService.GetAllAsync();
            return Ok(radars);
        }
        // 🔓 Radar: Get vehicle by plate
        /// <summary>
        /// Get a radar by its ID
        /// </summary>
        /// <param name="id">radar id</param>
        /// <returns></returns>
        [HttpGet("radar/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var radar = await _radarService.GetByIdAsync(id);
            if (radar == null)
            {
                return NotFound();
            }
            return Ok(radar);
        }
        /// <summary>
        /// create new radar
        /// </summary>
        /// <param name="radarDto"></param>
        /// <returns></returns>
        // 🔒 Radar: Create radar
        [HttpPost("radar")]

        public async Task<IActionResult> CreateRadar([FromBody] RadarDto radarDto)
        {
            if (radarDto == null)
            {
                return BadRequest("Invalid radar data.");
            }
            var createdRadar = await _radarService.CreateAsync(radarDto);
            return CreatedAtAction(nameof(GetById), new { id = createdRadar.RadarId }, createdRadar);
        }
        
    }
}
