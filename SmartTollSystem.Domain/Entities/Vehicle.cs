using SmartTollSystem.Domain.Entities.Enum;
using SmartTollSystem.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Domain.Entities
{
    public class Vehicle
    {
        [Key]
        public Guid VehicleId { get; set; }

        [Required(ErrorMessage = "License plate is required")]
        [RegularExpression(@"^\d{3}[A-Z]{3}$", ErrorMessage = "License plate must be in format 123ABC")]
        public string LicensePlate { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vehicle type is required")]
        public VehicleType VehicleType { get; set; }

        public bool NeedsConfirmation { get; set; }
        public string Type { get; set; } = string.Empty;

        public Guid? OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public ApplicationUser? Owner { get; set; }

        public ICollection<TollHistory> TollHistories { get; set; } = new List<TollHistory>();
        public ICollection<Detection> Detections { get; set; } = new List<Detection>();
    }

}
