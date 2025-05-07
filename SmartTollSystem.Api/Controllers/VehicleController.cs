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

        /// <summary>
        /// Get vehicle by plate number
        /// </summary>
        /// <param name="plate"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Get vehicle by logged in user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Get all vehicles
        /// </summary>
        /// <returns></returns>

        [HttpGet("vehicles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllVehicles()
        {
            var vehicles = await _vehicleService.GetAllVehiclesAsync();
            return Ok(vehicles);
        }
        /// <summary>
        /// Get vehicles by user ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("user/{userId}/vehicles")]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetVehiclesByUser(Guid userId)
        {
            var vehicles = await _vehicleService.GetVehiclesByUserAsync(userId);
            return Ok(vehicles);
        }
        /// <summary>
        /// Register a new vehicle
        /// </summary>
        /// <param name="vehicleDto"></param>
        /// <returns></returns>


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

        /// <summary>
        /// Delete a vehicle by plate number
        /// </summary>
        /// <param name="plate"></param>
        /// <returns></returns>

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
        /// <summary>
        /// Update vehicle details
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <param name="vehicleDto"></param>
        /// <returns></returns>
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
