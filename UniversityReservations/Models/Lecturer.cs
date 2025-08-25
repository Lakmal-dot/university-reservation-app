using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UniversityReservations.Models
{
    public class Lecturer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string? LastName { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        // Foreign key
        public int SubjectId { get; set; }

        // Navigation properties
        [JsonIgnore]
        public Subject? Subject { get; set; }

        [JsonIgnore]
        public ICollection<Reservation>? Reservations { get; set; }

        [NotMapped]
        [JsonIgnore]
        public string FullName => $"{FirstName} {LastName}";
    }
}
