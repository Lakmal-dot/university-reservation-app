using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UniversityReservations.Data;
using UniversityReservations.Models;

namespace UniversityReservations.Pages.LectureHalls
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public LectureHall LectureHall { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            LectureHall = await _context.LectureHalls.FirstOrDefaultAsync(m => m.Id == id);

            if (LectureHall == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
