using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Domain.DTOs
{
    public class TollHistoryDto
    {
        public string PlateNumber { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public decimal Amount { get; set; }
        public string Location { get; set; } = string.Empty;
    }
}
