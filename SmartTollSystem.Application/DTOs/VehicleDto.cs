using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Domain.DTOs
{
    public record VehicleDto
    {
        public string PlateNumber { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public Guid OwnerId { get; set; }
    }
}
