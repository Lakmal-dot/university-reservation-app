using UniversityReservations.Models;

namespace UniversityReservations.Services
{
    public interface IReservationService
    {
        Task<Reservation> GetNextReservationForLecturer(int lecturerId);
        Task<IEnumerable<Reservation>> GetFutureReservationsForLecturer(int lecturerId);
        Task<IEnumerable<Reservation>> GetPastReservationsForLecturer(int lecturerId);
        Task<bool> IsTimeSlotAvailable(int lectureHallId, DateTime startTime, DateTime endTime, int? excludeReservationId = null);
    }
}
