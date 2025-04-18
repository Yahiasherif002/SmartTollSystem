using SmartTollSystem.Application.Contracts;
using SmartTollSystem.Application.DTOs;
using SmartTollSystem.Domain.Entities;
using SmartTollSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Application.Services
{
    public class RadarService : IRadarService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RadarService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<RadarDto> CreateAsync(RadarDto radarDto)
        {
            var radar = new Radar
            {
                Location=radarDto.Location,
                EmergencyPriority=radarDto.EmergencyPriority
            };
            await _unitOfWork.RadarRepository.AddAsync(radar);
            await _unitOfWork.SaveAsync();
            radarDto.RadarId = radar.RadarId;
            return radarDto;
        }

        public async Task<bool> DeleteAsync(Guid radarId)
        {
            var radar = await _unitOfWork.RadarRepository.GetByIdAsync(radarId);
            if (radar == null) return false;
            await _unitOfWork.RadarRepository.DeleteAsync(radarId);
            await _unitOfWork.SaveAsync();
            return true;

        }

        public async Task<IEnumerable<RadarDto>> GetAllAsync()
        {
            var radars = await _unitOfWork.RadarRepository.GetAllAsync();
            return radars.Select(r => new RadarDto
            {
                RadarId = r.RadarId,
                Location = r.Location,
                EmergencyPriority = r.EmergencyPriority
            });
        }

        public async Task<RadarDto?> GetByIdAsync(Guid radarId)
        {
            var radar = await _unitOfWork.RadarRepository.GetByIdAsync(radarId);
            if (radar == null) return null;
            return new RadarDto
            {
                RadarId = radar.RadarId,
                Location = radar.Location,
                EmergencyPriority = radar.EmergencyPriority
            };
        }
    }
}
