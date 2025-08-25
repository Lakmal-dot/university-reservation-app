using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UniversityReservations.Data;
using UniversityReservations.Models;

namespace UniversityReservations.Pages.LectureHalls
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public LectureHall LectureHall { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.LectureHalls.Add(LectureHall);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
