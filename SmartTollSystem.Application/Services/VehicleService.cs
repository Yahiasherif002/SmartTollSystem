using SmartTollSystem.Domain.DTOs;
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
        public Task<bool> DeleteVehicleAsync(string plate)
        {
            throw new NotImplementedException();
        }

        public Task<VehicleDto?> GetVehicleByPlateAsync(string plate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<VehicleDto>> GetVehiclesByUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<VehicleDto> RegisterVehicleAsync(VehicleDto vehicleDto)
        {
            throw new NotImplementedException();
        }
    }
}
