using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartTollSystem.Domain.DTOs;
using SmartTollSystem.Domain.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartTollSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize(Roles = "Admin")]

    public class DashboardController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        /// <summary>
        ///     Get the count of all users
        /// </summary>
        /// <returns></returns>
        [HttpGet("users/count")]
        public async Task<IActionResult> GetUserCount()
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            return Ok(users.Count());
        }
        /// <summary>
        ///   Get all users with their vehicles
        /// </summary>
        /// <returns></returns>
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersWithVehiclesAsync();
            return Ok(users);
        }
        /// <summary>
        ///   Get the count of all vehicles
        /// </summary>
        /// <returns></returns>
        [HttpGet("vehicles/count")]
        public async Task<IActionResult> GetVehicleCount()
        {
            var vehicles = await _unitOfWork.VehicleRepository.GetAllAsync();
            return Ok(vehicles.Count());
        }
        /// <summary>
        ///   Get the count of all tolls
        /// </summary>
        /// <returns></returns>

        [HttpGet("tolls/count")]
        public async Task<IActionResult> GetTollCount()
        {
            var tolls = await _unitOfWork.TollRepository.GetAllAsync();
            return Ok(tolls.Count());
        }
        /// <summary>
        ///   Get the total revenue from all tolls
        /// </summary>
        /// <returns></returns>

        [HttpGet("revenue/total")]
        public async Task<IActionResult> GetTotalRevenue()
        {
            var tolls = await _unitOfWork.TollRepository.GetAllAsync();
            var revenue = tolls.Sum(t => t.TollAmount);
            return Ok(revenue);
        }
        /// <summary>
        ///   Get the total revenue from tolls today
        /// </summary>
        /// <returns></returns>
        [HttpGet("revenue/today")]
        public async Task<IActionResult> GetTodayRevenue()
        {
            var today = DateTime.UtcNow.Date;
            var tolls = await _unitOfWork.TollRepository.GetAllAsync();
            var todayRevenue = tolls
                .Where(t => t.Timestamp.Date == today)
                .Sum(t => t.TollAmount);
            return Ok(todayRevenue);
        }
        /// <summary>
        ///   Get the count of transactions today
        /// </summary>
        /// <returns></returns>

        [HttpGet("transactions/today")]
        public async Task<IActionResult> GetTodayTransactionCount()
        {
            var today = DateTime.UtcNow.Date;
            var tolls = await _unitOfWork.TollRepository.GetAllAsync();
            var count = tolls.Count(t => t.Timestamp.Date == today);
            return Ok(count);
        }
    }
}
