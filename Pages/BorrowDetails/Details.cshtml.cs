using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LibrarySystem.Models;

namespace LibrarySystem.Pages.BorrowDetails
{
    public class DetailsModel : PageModel
    {
        private readonly LibrarySystem.Models.LibrarySystemContext _context;

        public DetailsModel(LibrarySystem.Models.LibrarySystemContext context)
        {
            _context = context;
        }

      public BorrowDetail BorrowDetail { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.BorrowDetails == null)
            {
                return NotFound();
            }

            var borrowdetail = await _context.BorrowDetails.FirstOrDefaultAsync(m => m.BorrowId == id);
            if (borrowdetail == null)
            {
                return NotFound();
            }
            else 
            {
                BorrowDetail = borrowdetail;
            }
            return Page();
        }
    }
}
