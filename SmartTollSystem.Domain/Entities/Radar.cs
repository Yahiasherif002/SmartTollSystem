using System.ComponentModel.DataAnnotations;

namespace SmartTollSystem.Domain.Entities
{
    public class Radar
    {
        [Key]
        public Guid RadarId { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Emergency priority is required")]
        public bool EmergencyPriority { get; set; }

        public ICollection<Detection> Detections { get; set; } = new List<Detection>();
    }

}