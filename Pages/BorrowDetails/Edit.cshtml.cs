using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library_System;
using Library_System.Models;

namespace Library_System.Pages.BorrowDetails
{
    public class EditModel : PageModel
    {
        private readonly Library_System.LibrarySystemContext _context;

        public EditModel(Library_System.LibrarySystemContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BorrowDetail BorrowDetail { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.BorrowDetails == null)
            {
                return NotFound();
            }

            var borrowdetail =  await _context.BorrowDetails.FirstOrDefaultAsync(m => m.BorrowId == id);
            if (borrowdetail == null)
            {
                return NotFound();
            }
            BorrowDetail = borrowdetail;
           ViewData["AccountId"] = new SelectList(_context.Accounts, "UserId", "UserId");
           ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(BorrowDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BorrowDetailExists(BorrowDetail.BorrowId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool BorrowDetailExists(string id)
        {
          return (_context.BorrowDetails?.Any(e => e.BorrowId == id)).GetValueOrDefault();
        }
    }
}
