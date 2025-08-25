using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UniversityReservations.Data;
using UniversityReservations.Models;

namespace UniversityReservations.Pages.LectureHalls
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            LectureHall = await _context.LectureHalls.FindAsync(id);

            if (LectureHall != null)
            {
                _context.LectureHalls.Remove(LectureHall);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
