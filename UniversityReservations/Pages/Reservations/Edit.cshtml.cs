using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityReservations.Data;
using UniversityReservations.Models;
using UniversityReservations.Services;

namespace UniversityReservations.Pages.Reservations
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IReservationService _reservationService;

        public EditModel(ApplicationDbContext context, IReservationService reservationService)
        {
            _context = context;
            _reservationService = reservationService;
        }

        [BindProperty]
        public Reservation Reservation { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Reservation = await _context.Reservations
                .Include(r => r.LectureHall)
                .Include(r => r.Lecturer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Reservation == null)
            {
                return NotFound();
            }

            ViewData["LectureHallId"] = new SelectList(_context.LectureHalls, "Id", "Name", Reservation.LectureHallId);
            ViewData["LecturerId"] = new SelectList(_context.Lecturers.Include(l => l.Subject), "Id", "FullName", Reservation.LecturerId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["LectureHallId"] = new SelectList(_context.LectureHalls, "Id", "Name", Reservation.LectureHallId);
                ViewData["LecturerId"] = new SelectList(_context.Lecturers.Include(l => l.Subject), "Id", "FullName", Reservation.LecturerId);
                return Page();
            }

            // Check if time slot is available
            var isAvailable = await _reservationService.IsTimeSlotAvailable(
                Reservation.LectureHallId,
                Reservation.StartTime,
                Reservation.EndTime,
                Reservation.Id
            );

            if (!isAvailable)
            {
                ModelState.AddModelError(string.Empty, "This time slot is not available. Please choose a different time or lecture hall.");
                ViewData["LectureHallId"] = new SelectList(_context.LectureHalls, "Id", "Name", Reservation.LectureHallId);
                ViewData["LecturerId"] = new SelectList(_context.Lecturers.Include(l => l.Subject), "Id", "FullName", Reservation.LecturerId);
                return Page();
            }

            // Ensure DateTime values are treated as UTC
            Reservation.StartTime = DateTime.SpecifyKind(Reservation.StartTime, DateTimeKind.Unspecified);
            Reservation.EndTime = DateTime.SpecifyKind(Reservation.EndTime, DateTimeKind.Unspecified);

            _context.Attach(Reservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(Reservation.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
