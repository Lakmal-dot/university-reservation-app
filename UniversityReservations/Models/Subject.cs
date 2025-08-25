using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UniversityReservations.Models
{
    public class Subject
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        // Navigation properties
        [JsonIgnore]
        public ICollection<Lecturer>? Lecturers { get; set; }
    }
}
