using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartTollSystem.Domain.Entities
{
    public class Detection
    {
        [Key]
        public Guid DetectionId { get; set; }

        [Required(ErrorMessage = "Timestamp is required")]
        public DateTime Timestamp { get; set; }

        public Guid RadarId { get; set; }

        [ForeignKey("RadarId")]
        public Radar? Radar { get; set; }
        public Guid VehicleId { get; set; }

        [ForeignKey("VehicleId")]
        public Vehicle? Vehicle { get; set; }
    }
}