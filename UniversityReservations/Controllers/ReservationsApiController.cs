using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityReservations.Data;
using UniversityReservations.Models;
using UniversityReservations.Services;

namespace UniversityReservations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsApiController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly ApplicationDbContext _context;

        public ReservationsApiController(IReservationService reservationService, ApplicationDbContext context)
        {
            _reservationService = reservationService;
            _context = context;
        }

        // get next reservation
        [HttpGet("NextReservation/{lecturerId}")]
        public async Task<ActionResult<Reservation>> GetNextReservation(int lecturerId)
        {
            var reservation = await _reservationService.GetNextReservationForLecturer(lecturerId);

            if (reservation == null)
            {
                return null;
            }

            return reservation;
        }

        // get future reservations
        [HttpGet("FutureReservations/{lecturerId}")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetFutureReservations(int lecturerId)
        {
            var now = DateTime.Now;
            var reservations = await _context.Reservations
                .Where(r => r.LecturerId == lecturerId && r.StartTime > now)
                .OrderBy(r => r.StartTime)
                .Include(r => r.LectureHall)
                .AsNoTracking()
                .ToListAsync();

            return Ok(reservations);
        }

        // get past reservations
        [HttpGet("PastReservations/{lecturerId}")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetPastReservations(int lecturerId)
        {
            var now = DateTime.Now;
            var reservations = await _context.Reservations
                .Where(r => r.LecturerId == lecturerId && r.EndTime < now)
                .OrderByDescending(r => r.StartTime)
                .Include(r => r.LectureHall)
                .AsNoTracking()
                .ToListAsync();

            return Ok(reservations);
        }

        // check availability of a time slot
        [HttpGet("CheckAvailability")]
        public async Task<ActionResult<bool>> CheckAvailability(int lectureHallId, DateTime startTime, DateTime endTime, int? excludeReservationId = null)
        {
            var isAvailable = await _reservationService.IsTimeSlotAvailable(lectureHallId, startTime, endTime, excludeReservationId);
            return Ok(new { available = isAvailable });
        }
    }
}
