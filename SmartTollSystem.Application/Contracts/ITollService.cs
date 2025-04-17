using SmartTollSystem.Application.DTOs;
using SmartTollSystem.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Domain.Interfaces
{
    public interface ITollService
    {
        Task<TollResultDto> ProcessTollAsync(LicensePlateDto licensePlateDto);
        Task<IEnumerable<TollHistoryDto>> GetTollHistoryAsync(Guid userId);
    }
}
