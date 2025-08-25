using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UniversityReservations.Data;
using UniversityReservations.Models;

namespace UniversityReservations.Pages.LectureHalls
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<LectureHall> LectureHalls { get; set; }

        public async Task OnGetAsync()
        {
            LectureHalls = await _context.LectureHalls
                .AsNoTracking()
                .OrderBy(lh => lh.Name)
                .ToListAsync();
        }
    }
}
