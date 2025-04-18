using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SmartTollSystem.Domain.DTOs
{
    public record UserDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [JsonIgnore]
        public string Role { get; set; } = "VEHICLEOWNER";

        public List<VehicleDto> Vehicles { get; set; }

    }
}
