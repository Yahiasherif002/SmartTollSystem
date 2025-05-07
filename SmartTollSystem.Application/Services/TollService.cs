using SmartTollSystem.Application.Contracts;
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
        private readonly IInvoiceService _invoiceService;
        public TollService(IUnitOfWork unitOfWork, IVehicleService vehicleService, IInvoiceService invoiceService)
        {
            _unitOfWork = unitOfWork;
            _vehicleService = vehicleService;
            _invoiceService = invoiceService;
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
            const decimal tollAmount = 15.00m;
            const decimal unregisteredFee = 50.00m;
            var locations = new[] { "Mansoura-gamsa", "AlesRoad", "BanhaST", "Al-Alamin" };
            var location = locations[new Random().Next(locations.Length)];

            var vehicleDto = await _vehicleService.GetVehicleByPlateAsync(licensePlateDto.PlateNumber);

            if (vehicleDto == null)
            {
                return await HandleUnregisteredVehicleAsync(licensePlateDto.PlateNumber, location, unregisteredFee);
            }

            if (vehicleDto.VehicleType == VehicleType.Emergency)
            {
                return HandleEmergencyVehicle(licensePlateDto.PlateNumber, location);
            }

            return await HandleRegisteredVehicleAsync(vehicleDto, tollAmount, location);
        }

        private async Task<TollResultDto> HandleUnregisteredVehicleAsync(string plateNumber, string location, decimal unregisteredFee)
        {
            var newVehicle = new Vehicle
            {
                VehicleId = Guid.NewGuid(),
                LicensePlate = plateNumber,
                VehicleType = VehicleType.Other,
                Balance = 0m,
                NeedsConfirmation = true
            };

            await _unitOfWork.VehicleRepository.AddAsync(newVehicle);

            var tollHistory = new TollHistory
            {
                VehicleId = newVehicle.VehicleId,
                Timestamp = DateTime.UtcNow,
                TollAmount = unregisteredFee,
                Location = location
            };

            await _unitOfWork.TollRepository.AddAsync(tollHistory);

            // Create invoice DTO
            var invoiceDto = new InvoiceDto
            {
                InvoiceId = Guid.NewGuid(),
                VehicleId = newVehicle.VehicleId,
                PlateNumber = plateNumber,
                Amount = unregisteredFee,
                Location = location,
                CreatedAt = DateTime.UtcNow,
                IsPaid = false,
                TollHistoryId = tollHistory.TollHistoryId
            };

            var createdInvoice = await _invoiceService.CreateInvoiceAsync(invoiceDto);

            await _unitOfWork.SaveAsync();

            NotifyAdmin(newVehicle);

            return new TollResultDto
            {
                PlateNumber = plateNumber,
                Success = false,
                Message = "Vehicle not found. Registered as new with penalty fee. Pending admin approval.",
                DeductedAmount = unregisteredFee,
                Date = DateTime.UtcNow,
                Location = location,
                InvoiceId = createdInvoice.InvoiceId
            };
        }

        private TollResultDto HandleEmergencyVehicle(string plateNumber, string location)
        {
            return new TollResultDto
            {
                PlateNumber = plateNumber,
                Success = true,
                Message = "Emergency vehicle – toll exempted.",
                DeductedAmount = 0m,
                Date = DateTime.UtcNow,
                Location = location
            };
        }

        private async Task<TollResultDto> HandleRegisteredVehicleAsync(VehicleDto vehicleDto, decimal tollAmount, string location)
        {
            var vehicleEntities = await _unitOfWork.VehicleRepository.FindAsync(v => v.VehicleId == vehicleDto.VehicleId);
            var vehicleEntity = vehicleEntities.FirstOrDefault();

            if (vehicleEntity == null || vehicleEntity.Balance < tollAmount)
            {
                return new TollResultDto
                {
                    PlateNumber = vehicleDto.PlateNumber,
                    Success = false,
                    Message = "Insufficient balance.",
                    DeductedAmount = null,
                    Date = DateTime.UtcNow,
                    Location = location
                };
            }

            vehicleEntity.Balance -= tollAmount;
            await _unitOfWork.VehicleRepository.UpdateAsync(vehicleEntity);

            var tollHistory = new TollHistory
            {
                VehicleId = vehicleEntity.VehicleId,
                Timestamp = DateTime.UtcNow,
                TollAmount = tollAmount,
                Location = location
            };

            await _unitOfWork.TollRepository.AddAsync(tollHistory);

            // Create invoice DTO
            var invoiceDto = new InvoiceDto
            {
                InvoiceId = Guid.NewGuid(),
                VehicleId = vehicleEntity.VehicleId,
                PlateNumber = vehicleEntity.LicensePlate,
                Amount = tollAmount,
                Location = location,
                CreatedAt = DateTime.UtcNow,
                IsPaid = false,
                TollHistoryId = tollHistory.TollHistoryId
            };

            var createdInvoice = await _invoiceService.CreateInvoiceAsync(invoiceDto);

            await _unitOfWork.SaveAsync();

            return new TollResultDto
            {
                PlateNumber = vehicleDto.PlateNumber,
                Success = true,
                Message = "Toll deducted successfully.",
                DeductedAmount = tollAmount,
                Date = DateTime.UtcNow,
                Location = location,
                InvoiceId = createdInvoice.InvoiceId
            };
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

        private void NotifyAdmin(Vehicle newVehicle)
        {
            Console.WriteLine($"New vehicle registered: {newVehicle.LicensePlate}. Needs admin confirmation.");

            // If  using email:
            // var emailService = new EmailService();
            // emailService.SendAdminNotification($"New vehicle registered: {newVehicle.LicensePlate}");
        }
    }
}