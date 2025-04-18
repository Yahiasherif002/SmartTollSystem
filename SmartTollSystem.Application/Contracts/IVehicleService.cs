using SmartTollSystem.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Domain.Interfaces
{
    public interface IVehicleService
    {
        Task<VehicleDto?> GetVehicleByPlateAsync(string plate);
        Task<IEnumerable<VehicleDto>> GetAllVehiclesAsync();
        Task<VehicleDto?> GetVehicleByIdAsync(Guid id);
        Task<VehicleDto?> GetVehicleByLoggedInUserAsync(Guid userId);

        Task<VehicleDto?> UpdatetVehicleAsync(Guid vehicleId);

        Task<IEnumerable<VehicleDto>> GetVehiclesByUserAsync(Guid userId);
        Task<VehicleDto> RegisterVehicleAsync(VehicleDto vehicleDto);
        Task<bool> DeleteVehicleAsync(string plate);
    }
}
