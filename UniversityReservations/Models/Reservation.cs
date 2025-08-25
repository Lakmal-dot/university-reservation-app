using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UniversityReservations.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [CustomValidation(typeof(Reservation), "ValidateEndTime")]
        public DateTime EndTime { get; set; }

        [StringLength(500)]
        public string? Purpose { get; set; }

        // Foreign keys
        public int LecturerId { get; set; }
        public int LectureHallId { get; set; }

        // Navigation properties
        [JsonIgnore]
        public Lecturer? Lecturer { get; set; }

        [JsonIgnore]
        public LectureHall? LectureHall { get; set; }

        public static ValidationResult ValidateEndTime(DateTime endTime, ValidationContext context)
        {
            var instance = context.ObjectInstance as Reservation;
            if (instance != null && endTime <= instance.StartTime)
            {
                return new ValidationResult("End time must be after start time");
            }
            return ValidationResult.Success;
        }
    }
}
