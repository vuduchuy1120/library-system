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
using Microsoft.AspNetCore.Authorization;

namespace Library_System.Pages.BorrowDetails
{
    [Authorize(Policy = "Admin")]
    public class EditModel : PageModel
    {
        private readonly Library_System.LibrarySystemContext _context;

        public EditModel(Library_System.LibrarySystemContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BorrowDetail BorrowDetail { get; set; } = default!;
        

        public async Task<IActionResult> OnGetAsync(int id)
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
           ViewData["AccountId"] = new SelectList(_context.Accounts, "UserId", "UserName");
           ViewData["BookId"] = new SelectList(_context.Books, "Id", "BookName");
            return Page();
        }

        public void loadData()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "UserId", "UserName");
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "BookName");
        }
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                loadData();
                return Page();
            }
            if(BorrowDetail.ReturnDate<BorrowDetail.BorrowDate)
            {
                loadData();
                ModelState.AddModelError("BorrowDetail.ReturnDate", "Return date must be greater than borrow date");
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

        private bool BorrowDetailExists(int id)
        {
          return (_context.BorrowDetails?.Any(e => e.BorrowId == id)).GetValueOrDefault();
        }
    }
}
