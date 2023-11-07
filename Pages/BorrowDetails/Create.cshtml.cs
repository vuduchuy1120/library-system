using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Library_System;
using Library_System.Models;

namespace Library_System.Pages.BorrowDetails
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
        ViewData["AccountId"] = new SelectList(_context.Accounts, "UserId", "UserId");
        ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public BorrowDetail BorrowDetail { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.BorrowDetails == null || BorrowDetail == null)
            {
                return Page();
            }

            _context.BorrowDetails.Add(BorrowDetail);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
