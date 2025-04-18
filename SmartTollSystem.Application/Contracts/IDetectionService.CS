using SmartTollSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Application.Contracts
{
    public interface IDetectionService
    {
        Task<DetectionDto> LogDetectionAsync(DetectionDto detectionDto);
        Task<IEnumerable<DetectionDto>> GetByVehicleAsync(Guid vehicleId);
        Task<IEnumerable<DetectionDto>> GetByRadarAsync(Guid radarId);
    }
}
