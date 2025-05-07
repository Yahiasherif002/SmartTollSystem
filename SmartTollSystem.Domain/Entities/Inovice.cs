using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Domain.Entities
{
    public class Invoice
    {
        public Guid InvoiceId { get; set; } = Guid.NewGuid();
        public Guid VehicleId { get; set; }
        public string PlateNumber { get; set; }
        public decimal Amount { get; set; }
        public string Location { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsPaid { get; set; } 
        public Guid? TollHistoryId { get; set; } // Link to toll history 

        public Vehicle Vehicle { get; set; }
        public TollHistory TollHistory { get; set; }
    }

}
