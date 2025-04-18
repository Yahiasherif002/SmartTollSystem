using SmartTollSystem.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Domain.DTOs
{
    public record VehicleDto
    {
        public Guid VehicleId { get; set; }
        public VehicleType VehicleType { get; set; }

        public string PlateNumber { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public Guid OwnerId { get; set; }
    }
}
