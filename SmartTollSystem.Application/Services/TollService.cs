using SmartTollSystem.Application.DTOs;
using SmartTollSystem.Domain.DTOs;
using SmartTollSystem.Domain.Entities;
using SmartTollSystem.Domain.Entities.Enum;
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
            const decimal tollAmount = 15.00m;
            const string location = "Toll Plaza 1";

            var vehicleDto = await _vehicleService.GetVehicleByPlateAsync(licensePlateDto.PlateNumber);

            if (vehicleDto == null)
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

            if (vehicleDto.VehicleType == VehicleType.Emergency)
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

            // Get actual Vehicle entity
            var vehicleEntity = (await _unitOfWork.VehicleRepository
                .FindAsync(v => v.VehicleId == vehicleDto.VehicleId)).FirstOrDefault();

            if (vehicleEntity == null)
            {
                return new TollResultDto
                {
                    PlateNumber = licensePlateDto.PlateNumber,
                    Success = false,
                    Message = "Vehicle data mismatch.",
                    DeductedAmount = null,
                    Date = DateTime.UtcNow,
                    Location = location
                };
            }

            if (vehicleEntity.Balance < tollAmount)
            {
                return new TollResultDto
                {
                    PlateNumber = licensePlateDto.PlateNumber,
                    Success = false,
                    Message = "Insufficient balance.",
                    DeductedAmount = null,
                    Date = DateTime.UtcNow,
                    Location = location
                };
            }

            // Deduct the balance
            vehicleEntity.Balance -= tollAmount;
            _unitOfWork.VehicleRepository.UpdateAsync(vehicleEntity);

            // Save toll history
            var tollHistory = new TollHistory
            {
                VehicleId = vehicleEntity.VehicleId,
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
        public async Task<TollResultDto> ProcessTollAsyncV1(LicensePlateDto licensePlateDto)
        {
            const decimal tollAmount = 15.00m; // الرسوم العادية للمركبات المسجلة
            const decimal unregisteredFee = 50.00m; // الغرامة للمركبات غير المسجلة
            var locations = new[] { "Mansoura-gamsa", "AlesRoad", "BanhaST", "Al-Alamin" }; // Array of locations
            var random = new Random();
            var location = locations[random.Next(locations.Length)]; // Select a random location

            var currentDateTime = DateTime.UtcNow;

            // محاولة الحصول على السيارة باستخدام لوحة الترخيص
            var vehicleDto = await _vehicleService.GetVehicleByPlateAsync(licensePlateDto.PlateNumber);

            // إذا لم يتم العثور على السيارة (مركبة غير مسجلة)
            if (vehicleDto == null)
            {
                // إذا كانت المركبة غير مسجلة، نفرض غرامة
                return new TollResultDto
                {
                    PlateNumber = licensePlateDto.PlateNumber,
                    Success = false,
                    Message = "Vehicle not registered. Additional fee applied.",
                    DeductedAmount = unregisteredFee,
                    Date = DateTime.UtcNow,
                    Location = location
                };
            }

            // إذا كانت المركبة من نوع طوارئ، يتم إعفاءها من الرسوم
            if (vehicleDto.VehicleType == VehicleType.Emergency)
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

            // الحصول على كيان السيارة الفعلي
            var vehicleEntity = (await _unitOfWork.VehicleRepository
                .FindAsync(v => v.VehicleId == vehicleDto.VehicleId)).FirstOrDefault();

            // إذا كانت السيارة مسجلة، نقوم بالتحقق من الرصيد
            if (vehicleEntity != null)
            {
                if (vehicleEntity.Balance < tollAmount)
                {
                    return new TollResultDto
                    {
                        PlateNumber = licensePlateDto.PlateNumber,
                        Success = false,
                        Message = "Insufficient balance.",
                        DeductedAmount = null,
                        Date = DateTime.UtcNow,
                        Location = location
                    };
                }

                // خصم الرسوم العادية للمركبات المسجلة
                vehicleEntity.Balance -= tollAmount;
                _unitOfWork.VehicleRepository.UpdateAsync(vehicleEntity);

                // حفظ تاريخ الرسوم
                var tollHistory = new TollHistory
                {
                    VehicleId = vehicleEntity.VehicleId,
                    Timestamp = currentDateTime,
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
            else
            {
                // إذا كانت السيارة غير مسجلة، نفرض الغرامة
                return new TollResultDto
                {
                    PlateNumber = licensePlateDto.PlateNumber,
                    Success = false,
                    Message = "Vehicle not found. Additional fee applied for unregistered vehicle.",
                    DeductedAmount = unregisteredFee,
                    Date = DateTime.UtcNow,
                    Location = location
                };
            }
        }



        public async Task<IEnumerable<TollHistoryDto>> GetAllTollsAsync()
        {
            var tolls = await _unitOfWork.TollRepository.GetAllAsync();
            return tolls.Select(t => new TollHistoryDto
            {
                PlateNumber = t.Vehicle?.LicensePlate ?? "Unknown",
                Amount = t.TollAmount,
                Timestamp = t.Timestamp,
                Location = t.Location
            });
        }

        public async Task<TollHistoryDto?> GetTollByIdAsync(Guid id)
        {
            var toll = await _unitOfWork.TollRepository.GetByIdAsync(id);
            if (toll == null) return null;

            return new TollHistoryDto
            {
                PlateNumber = toll.Vehicle?.LicensePlate ?? "Unknown",
                Amount = toll.TollAmount,
                Timestamp = toll.Timestamp,
                Location = toll.Location
            };
        }
    }
}
