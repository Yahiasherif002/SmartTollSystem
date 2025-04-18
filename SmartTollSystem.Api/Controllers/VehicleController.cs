using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartTollSystem.Domain.DTOs;
using SmartTollSystem.Domain.Interfaces;
using System.Security.Claims;

namespace SmartTollSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }


        [HttpGet("{plate}")]
        public async Task<IActionResult> GetVehicleByPlate(string plate)
        {
            var vehicle = await _vehicleService.GetVehicleByPlateAsync(plate);
            if (vehicle == null)
            {
                return NotFound();
            }
            return Ok(vehicle);
        }
        [Authorize] 
        [HttpGet("user/{userId}/vehicle")]
        public async Task<IActionResult> GetLoggedInUserVehicle(Guid userId)
        {
            var vehicle = await _vehicleService.GetVehicleByLoggedInUserAsync(userId);
            if (vehicle == null)
            {
                return NotFound();
            }
            return Ok(vehicle);

        }
        
        [HttpGet("vehicles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllVehicles()
        {
            var vehicles = await _vehicleService.GetAllVehiclesAsync();
            return Ok(vehicles);
        }

        [HttpGet("user/{userId}/vehicles")]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetVehiclesByUser(Guid userId)
        {
            var vehicles = await _vehicleService.GetVehiclesByUserAsync(userId);
            return Ok(vehicles);
        }


        [HttpPost("Register")]
        [Authorize]
        public async Task<IActionResult> RegisterVehicle([FromBody] VehicleDto vehicleDto)
        {
            var userIdClaim = User.FindFirst("sub") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            vehicleDto.OwnerId = userId;

            if (vehicleDto == null)
            {
                return BadRequest();
            }
            var createdVehicle = await _vehicleService.RegisterVehicleAsync(vehicleDto);
            return CreatedAtAction(nameof(GetVehicleByPlate), new { plate = createdVehicle.PlateNumber }, createdVehicle);
        }


        [HttpDelete("{plate}")]
        public async Task<IActionResult> DeleteVehicle(string plate)
        {
            var result = await _vehicleService.DeleteVehicleAsync(plate);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpPut("{vehicleId}")]
        public async Task<IActionResult> UpdateVehicle(Guid vehicleId, [FromBody] VehicleDto vehicleDto)
        {
            if (vehicleDto == null)
            {
                return BadRequest();
            }
            var updatedVehicle = await _vehicleService.UpdatetVehicleAsync(vehicleId);
            if (updatedVehicle == null)
            {
                return NotFound();
            }
            return Ok(updatedVehicle);

        }



    }
}
