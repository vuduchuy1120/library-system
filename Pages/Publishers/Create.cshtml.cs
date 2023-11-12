using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Library_System;
using Library_System.Models;
using Microsoft.AspNetCore.Authorization;

namespace Library_System.Pages.Publishers
{
    [Authorize(Policy = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly Library_System.LibrarySystemContext _context;

        public CreateModel(Library_System.LibrarySystemContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Publisher Publisher { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Publishers == null || Publisher == null)
            {
                return Page();
            }

            try
            {
                _context.Publishers.Add(Publisher);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("Publisher.PublisherName", "This Publisher Name is existed!!!");
                return Page(); 
            }
        }
    }
}
