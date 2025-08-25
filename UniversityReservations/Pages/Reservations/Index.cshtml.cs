using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UniversityReservations.Data;
using UniversityReservations.Models;

namespace UniversityReservations.Pages.Reservations
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Reservation> Reservations { get; set; }
        public IList<Lecturer> Lecturers { get; set; }

        public async Task OnGetAsync()
        {
            Reservations = await _context.Reservations
                .AsNoTracking()
                .Include(r => r.Lecturer)
                .Include(r => r.LectureHall)
                .OrderBy(r => r.StartTime)
                .ToListAsync();

            Lecturers = await _context.Lecturers
                .AsNoTracking()
                .OrderBy(l => l.LastName)
                .ThenBy(l => l.FirstName)
                .ToListAsync();
        }
    }
}
