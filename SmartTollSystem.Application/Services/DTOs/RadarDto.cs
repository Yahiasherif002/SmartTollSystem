using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Application.DTOs
{
    public class RadarDto
    {
        public Guid RadarId { get; set; }
        public string Location { get; set; } = string.Empty;
        public bool EmergencyPriority { get; set; }
    }
}
