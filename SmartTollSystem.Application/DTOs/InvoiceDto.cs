using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Application.DTOs
{
    public class InvoiceDto
    {
        public Guid InvoiceId { get; set; } // Invoice unique ID
        public Guid VehicleId { get; set; } // Vehicle ID
        public string PlateNumber { get; set; } // Vehicle's plate number
        public decimal Amount { get; set; } // Amount of the toll
        public string Location { get; set; } // Location of the toll charge
        public DateTime CreatedAt { get; set; } // Date of the invoice
        public bool IsPaid { get; set; } // Whether the invoice has been paid
        public Guid? TollHistoryId { get; set; } // Optional link to the toll history record
    }


}
