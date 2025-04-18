using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Application.DTOs
{
    public class DetectionDto
    {
        public Guid DetectionId { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid RadarId { get; set; }
        public Guid VehicleId { get; set; }
    }
}
