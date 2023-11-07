using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Library_System;
using Library_System.Models;

namespace Library_System.Pages.Categories
{
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
        public Category Category { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Categories == null || Category == null)
            {
                return Page();
            }

            try
            {
                _context.Categories.Add(Category);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }
            catch (Exception)
            {

                ModelState.AddModelError("Category.CategoryName", "This Category Name existed!!!");
                return Page();
            }
        }
    }
}
