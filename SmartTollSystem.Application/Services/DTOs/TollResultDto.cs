using SmartTollSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Application.DTOs
{
    public class TollResultDto
    {
        public string PlateNumber { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public decimal? DeductedAmount { get; set; }
        public DateTime? Date { get; set; }
        public string Location { get; set; } = string.Empty;

        public Guid? InvoiceId { get; set; }

    }
}
