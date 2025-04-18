using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartTollSystem.Domain.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartTollSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class DashboardController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("users/count")]
        public async Task<IActionResult> GetUserCount()
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            return Ok(users.Count());
        }
        [HttpGet("users")]
        public async Task<IActionResult> GetUser()
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            return Ok(users);
        }
        [HttpGet("vehicles/count")]
        public async Task<IActionResult> GetVehicleCount()
        {
            var vehicles = await _unitOfWork.VehicleRepository.GetAllAsync();
            return Ok(vehicles.Count());
        }

        [HttpGet("tolls/count")]
        public async Task<IActionResult> GetTollCount()
        {
            var tolls = await _unitOfWork.TollRepository.GetAllAsync();
            return Ok(tolls.Count());
        }

        [HttpGet("revenue/total")]
        public async Task<IActionResult> GetTotalRevenue()
        {
            var tolls = await _unitOfWork.TollRepository.GetAllAsync();
            var revenue = tolls.Sum(t => t.TollAmount);
            return Ok(revenue);
        }

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
