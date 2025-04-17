using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Domain.DTOs
{
    public record LicensePlateDto
    {
        public string PlateNumber { get; set; } = string.Empty;
       
    }
}
