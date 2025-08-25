using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UniversityReservations.Models
{
    public class LectureHall
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [Range(1, 1000)]
        public int Capacity { get; set; }

        public bool HasProjector { get; set; }
        public bool HasMicrophone { get; set; }

        [StringLength(200)]
        public string? Location { get; set; }

        // Navigation properties
        [JsonIgnore]
        public ICollection<Reservation>? Reservations { get; set; }
    }
}
