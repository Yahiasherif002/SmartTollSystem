using SmartTollSystem.Application.Contracts;
using SmartTollSystem.Application.DTOs;
using SmartTollSystem.Domain.Entities;
using SmartTollSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Application.Services
{
    public class DetectionService : IDetectionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DetectionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DetectionDto>> GetByVehicleAsync(Guid vehicleId)
        {
            return await GetDetectionsAsync(d => d.VehicleId == vehicleId);
        }

        public async Task<IEnumerable<DetectionDto>> GetByRadarAsync(Guid radarId)
        {
            return await GetDetectionsAsync(d => d.RadarId == radarId);
        }

        private async Task<IEnumerable<DetectionDto>> GetDetectionsAsync(Expression<Func<Detection, bool>> predicate)
        {
            var detections = await _unitOfWork.DetectionRepository.FindAsync(predicate);
            return detections.Select(d => new DetectionDto
            {
                DetectionId = d.DetectionId,
                Timestamp = d.Timestamp,
                RadarId = d.RadarId,
                VehicleId = d.VehicleId
            });
        }

        public async Task<DetectionDto> LogDetectionAsync(DetectionDto detectionDto)

        { 
            var detection = new Detection
            {
                Timestamp = detectionDto.Timestamp,
                RadarId = detectionDto.RadarId,
                VehicleId = detectionDto.VehicleId

            };
            await _unitOfWork.DetectionRepository.AddAsync(detection);
            await _unitOfWork.SaveAsync();
            detectionDto.DetectionId = detection.DetectionId;
            return detectionDto;
        }
    }
}
