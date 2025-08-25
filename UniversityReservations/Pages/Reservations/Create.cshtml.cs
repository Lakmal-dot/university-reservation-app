using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityReservations.Data;
using UniversityReservations.Models;
using UniversityReservations.Services;

namespace UniversityReservations.Pages.Reservations
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IReservationService _reservationService;

        public CreateModel(ApplicationDbContext context, IReservationService reservationService)
        {
            _context = context;
            _reservationService = reservationService;
        }

        public IActionResult OnGet()
        {
            ViewData["LectureHallId"] = new SelectList(_context.LectureHalls, "Id", "Name");
            ViewData["LecturerId"] = new SelectList(
                                                        _context.Lecturers
                                                            .Select(l => new { l.Id, FullName = l.FirstName + " " + l.LastName }),
                                                        "Id", "FullName"
                                                    );
            return Page();
        }

        [BindProperty]
        public Reservation Reservation { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["LectureHallId"] = new SelectList(_context.LectureHalls, "Id", "Name");
                ViewData["LecturerId"] = new SelectList(
                    _context.Lecturers
                        .Select(l => new { l.Id, FullName = l.FirstName + " " + l.LastName }),
                    "Id", "FullName"
                );
                return Page();
            }

            // Ensure DateTime values are UTC before checking availability
            var startUtc = DateTime.SpecifyKind(Reservation.StartTime, DateTimeKind.Utc);
            var endUtc = DateTime.SpecifyKind(Reservation.EndTime, DateTimeKind.Utc);

            // Check if time slot is available
            var isAvailable = await _reservationService.IsTimeSlotAvailable(
                Reservation.LectureHallId,
                startUtc,
                endUtc
            );

            if (!isAvailable)
            {
                ModelState.AddModelError(string.Empty, "This time slot is not available. Please choose a different time or lecture hall.");
                ViewData["LectureHallId"] = new SelectList(_context.LectureHalls, "Id", "Name");
                ViewData["LecturerId"] = new SelectList(
                    _context.Lecturers
                        .Select(l => new { l.Id, FullName = l.FirstName + " " + l.LastName }),
                    "Id", "FullName"
                );
                return Page();
            }

            // Set Unspecified before saving
            Reservation.StartTime = DateTime.SpecifyKind(startUtc, DateTimeKind.Unspecified);
            Reservation.EndTime = DateTime.SpecifyKind(endUtc, DateTimeKind.Unspecified);

            _context.Reservations.Add(Reservation);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
