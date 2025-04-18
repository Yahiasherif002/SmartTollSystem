using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Application.DTOs
{
    public class TopUpDto
    {
        public Guid VehicleId { get; set; }
        public decimal Amount { get; set; }
    }
}
