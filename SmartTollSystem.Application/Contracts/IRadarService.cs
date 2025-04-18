using SmartTollSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Application.Contracts
{
    public interface IRadarService
    {
        Task<IEnumerable<RadarDto>> GetAllAsync();
        Task<RadarDto?> GetByIdAsync(Guid radarId);
        Task<RadarDto> CreateAsync(RadarDto radarDto);
        Task<bool> DeleteAsync(Guid radarId);
    }
}
