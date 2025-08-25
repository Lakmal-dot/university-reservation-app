using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UniversityReservations.Data;
using UniversityReservations.Models;

namespace UniversityReservations.Pages.Lecturers
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Lecturer> Lecturers { get; set; }

        public async Task OnGetAsync()
        {
            Lecturers = await _context.Lecturers
                .AsNoTracking()
                .Include(l => l.Subject)
                .OrderBy(l => l.LastName)
                .ThenBy(l => l.FirstName)
                .ToListAsync();
        }
    }
}
