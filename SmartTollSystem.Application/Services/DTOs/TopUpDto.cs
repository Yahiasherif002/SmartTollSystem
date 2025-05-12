using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SmartTollSystem.Application.DTOs
{
    public class TopUpDto
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
