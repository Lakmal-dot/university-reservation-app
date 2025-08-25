using Microsoft.EntityFrameworkCore;
using UniversityReservations.Data;
using UniversityReservations.Interfaces;
using UniversityReservations.Models;

namespace UniversityReservations.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;

        public ReservationService(IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<Reservation> GetNextReservationForLecturer(int lecturerId)
        {
            var now = DateTime.Now;

            var nextReservation = await _context.Reservations
                .Where(r => r.LecturerId == lecturerId && r.StartTime > now)
                .OrderBy(r => r.StartTime)
                .Include(r => r.LectureHall)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return nextReservation;
        }

        public async Task<IEnumerable<Reservation>> GetFutureReservationsForLecturer(int lecturerId)
        {
            var now = DateTime.UtcNow;

            return await _context.Reservations
                .Where(r => r.LecturerId == lecturerId && r.StartTime > now)
                .OrderBy(r => r.StartTime)
                .Include(r => r.LectureHall)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetPastReservationsForLecturer(int lecturerId)
        {
            var now = DateTime.UtcNow;

            return await _context.Reservations
                .Where(r => r.LecturerId == lecturerId && r.EndTime < now)
                .OrderByDescending(r => r.StartTime)
                .Include(r => r.LectureHall)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> IsTimeSlotAvailable(int lectureHallId, DateTime startTime, DateTime endTime, int? excludeReservationId = null)
        {
            // Ensure UTC
            if (startTime.Kind == DateTimeKind.Unspecified)
                startTime = DateTime.SpecifyKind(startTime, DateTimeKind.Utc);
            else
                startTime = startTime.ToUniversalTime();

            if (endTime.Kind == DateTimeKind.Unspecified)
                endTime = DateTime.SpecifyKind(endTime, DateTimeKind.Utc);
            else
                endTime = endTime.ToUniversalTime();

            //  query to check for overlapping reservations
            var overlappingReservations = await _context.Reservations
                .FromSqlRaw(@"SELECT * FROM ""Reservations"" 
                              WHERE ""LectureHallId"" = {0} 
                              AND ""Id"" != {1}
                              AND ((""StartTime"" < {2} AND ""EndTime"" > {3}) 
                                   OR (""StartTime"" < {2} AND ""EndTime"" > {2}) 
                                   OR (""StartTime"" < {3} AND ""EndTime"" > {3}) 
                                   OR (""StartTime"" >= {2} AND ""EndTime"" <= {3}))",
                            lectureHallId, excludeReservationId ?? 0, endTime, startTime)
                .CountAsync();

            return overlappingReservations == 0;
        }
    }
}
