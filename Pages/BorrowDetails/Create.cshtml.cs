using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LibrarySystem.Models;

namespace LibrarySystem.Pages.BorrowDetails
{
    public class CreateModel : PageModel
    {
        private readonly LibrarySystem.Models.LibrarySystemContext _context;

        public CreateModel(LibrarySystem.Models.LibrarySystemContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id");
        ViewData["UserId"] = new SelectList(_context.Accounts, "UserId", "UserId");
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
