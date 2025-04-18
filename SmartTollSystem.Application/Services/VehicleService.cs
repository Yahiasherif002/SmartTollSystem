using Microsoft.AspNetCore.Mvc;
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
    public class VehicleService : IVehicleService
    {
       private readonly IUnitOfWork _unitOfWork;
        public VehicleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> DeleteVehicleAsync(string plate)
        {
            var vehicles = await _unitOfWork.VehicleRepository.FindAsync(p => p.LicensePlate == plate.ToUpper());
            var vehicle = vehicles.FirstOrDefault();
           
            if (vehicle == null) return false;

            await _unitOfWork.VehicleRepository.DeleteAsync(vehicle.VehicleId);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<VehicleDto>> GetAllVehiclesAsync()
        {
            var vehicles = await _unitOfWork.VehicleRepository.GetAllAsync();
            var vehicleDtos = vehicles.Select(v => new VehicleDto
            {
                VehicleId = v.VehicleId,
                PlateNumber = v.LicensePlate,
                Type = v.Type.ToString(),
                OwnerId = v.OwnerId ?? Guid.Empty,
            });
            return  vehicleDtos;
        }

        public Task<VehicleDto?> GetVehicleByIdAsync(Guid id)
        {
            var vehicles = _unitOfWork.VehicleRepository.FindAsync(v => v.VehicleId == id);
            var vehicle = vehicles.Result.FirstOrDefault();
            if (vehicle == null)
                return Task.FromResult<VehicleDto?>(null);
            return Task.FromResult(new VehicleDto
            {
                VehicleId = vehicle.VehicleId,
                PlateNumber = vehicle.LicensePlate,
                Type = vehicle.Type.ToString(),
                OwnerId = vehicle.OwnerId ?? Guid.Empty,
            });
        }

        public async Task<VehicleDto?> GetVehicleByLoggedInUserAsync(Guid userId)
        {
            var vehicles = await _unitOfWork.VehicleRepository.FindAsync(v => v.OwnerId == userId);
            var vehicle = vehicles.FirstOrDefault();
            if (vehicle == null)
                return null; 
            return new VehicleDto
            {
                VehicleId = vehicle.VehicleId,
                PlateNumber = vehicle.LicensePlate,
                Type = vehicle.Type.ToString(),
                OwnerId = vehicle.OwnerId ?? Guid.Empty,
            };
        }

        public async Task<VehicleDto?> GetVehicleByPlateAsync(string plate)
        {
            var vehicles = await _unitOfWork.VehicleRepository
           .FindAsync(v => v.LicensePlate == plate.ToLower());

            var vehicle = vehicles.FirstOrDefault();
            if (vehicle == null)
                return null;

            return new VehicleDto
            {
                VehicleId = vehicle.VehicleId,
                PlateNumber = vehicle.LicensePlate,
                Type = vehicle.Type.ToString(),
                OwnerId = vehicle.OwnerId ?? Guid.Empty,
                Balance = vehicle.Balance

            };
        }

        public async Task<IEnumerable<VehicleDto>> GetVehiclesByUserAsync(Guid userId)
        {
            var vehicles = await _unitOfWork.VehicleRepository.FindAsync(v => v.OwnerId == userId);
            var vehicleDtos = vehicles.Select(v => new VehicleDto
            {
                VehicleId = v.VehicleId,
                PlateNumber = v.LicensePlate,
                Type = v.Type.ToString(),
                OwnerId = v.OwnerId ?? Guid.Empty,
            });
            return vehicleDtos;
        }

        public async Task<VehicleDto> RegisterVehicleAsync(VehicleDto vehicleDto)
        {
            var owner = await _unitOfWork.UserRepository.GetByIdAsync(vehicleDto.OwnerId);
            if (owner == null)
                throw new Exception("Owner not found");
            var vehicle = new Vehicle
            {
                VehicleId = vehicleDto.VehicleId,
                LicensePlate = vehicleDto.PlateNumber.ToUpper(),
                VehicleType = vehicleDto.VehicleType, 
                OwnerId = vehicleDto.OwnerId,
                Type = vehicleDto.Type,
            };
            await _unitOfWork.VehicleRepository.AddAsync(vehicle);
            await _unitOfWork.SaveAsync();
            vehicleDto.VehicleId = vehicle.VehicleId;
            return vehicleDto;
        }

        public async Task<VehicleDto?> UpdatetVehicleAsync(Guid vehicleId)
        {
            var vehicles = await _unitOfWork.VehicleRepository.FindAsync(v => v.VehicleId == vehicleId);
            var vehicle = vehicles.FirstOrDefault();
            if (vehicle == null)
                return null;
            return new VehicleDto
            {
                VehicleId = vehicle.VehicleId,
                PlateNumber = vehicle.LicensePlate,
                Type = vehicle.Type.ToString(),
                OwnerId = vehicle.OwnerId ?? Guid.Empty,
            };
        }
    }
}
