using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartTollSystem.Domain.Entities
{
    public class TollHistory
    {
        [Key]
        public Guid TollHistoryId { get; set; }

        [Required(ErrorMessage = "Toll amount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Toll amount must be non-negative")]
        public decimal TollAmount { get; set; }

        [Required(ErrorMessage = "Timestamp is required")]
        public DateTime Timestamp { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; } = string.Empty;

        public Guid VehicleId { get; set; }

        [ForeignKey("VehicleId")]
        public Vehicle? Vehicle { get; set; }
    }
}