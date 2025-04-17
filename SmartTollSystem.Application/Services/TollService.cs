using SmartTollSystem.Application.DTOs;
using SmartTollSystem.Domain.DTOs;
using SmartTollSystem.Domain.Entities;
using SmartTollSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Application.Services
{
    public class TollService : ITollService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVehicleService _vehicleService;
        public TollService(IUnitOfWork unitOfWork, IVehicleService vehicleService)
        {
            _unitOfWork = unitOfWork;
            _vehicleService = vehicleService;
        }

        public async Task<IEnumerable<TollHistoryDto>> GetTollHistoryAsync(Guid userId)
        {
            var tollHistories = await _unitOfWork.TollRepository.GetAllAsync();
            var filteredHistories = tollHistories
                .Where(th => th.Vehicle != null && th.Vehicle.VehicleId == userId)
                .Select(th => new TollHistoryDto
                {
                    PlateNumber = th.Vehicle.LicensePlate,
                    Amount = th.TollAmount,
                    Timestamp = th.Timestamp,
                    Location = th.Location
                });
            return filteredHistories;
        }

        public async Task<TollResultDto> ProcessTollAsync(LicensePlateDto licensePlateDto)
        {
            var vehicle = await _vehicleService.GetVehicleByPlateAsync(licensePlateDto.PlateNumber);
            const decimal tollAmount = 5.00m;
            const string location = "Toll Plaza 1";
            if (vehicle == null)
            {
                return new TollResultDto
                {
                    PlateNumber = licensePlateDto.PlateNumber,
                    Success = false,
                    Message = "Vehicle not found.",
                    DeductedAmount = null,
                    Date = DateTime.UtcNow,
                    Location = location
                };
            }

            if (vehicle.VehicleType == VehicleType.Emergency)
            {
                return new TollResultDto
                {
                    PlateNumber = licensePlateDto.PlateNumber,
                    Success = true,
                    Message = "Emergency vehicle – toll exempted.",
                    DeductedAmount = 0m,
                    Date = DateTime.UtcNow,
                    Location = location
                };
            }

            var tollHistory = new TollHistory
            {
                VehicleId = vehicle.VehicleId,
                Timestamp = DateTime.UtcNow,
                TollAmount = tollAmount,
                Location = location
            };

            await _unitOfWork.TollRepository.AddAsync(tollHistory);
            await _unitOfWork.SaveAsync();

            return new TollResultDto
            {
                PlateNumber = licensePlateDto.PlateNumber,
                Success = true,
                Message = "Toll deducted successfully.",
                DeductedAmount = tollAmount,
                Date = tollHistory.Timestamp,
                Location = location
            };


        }
    }
}
