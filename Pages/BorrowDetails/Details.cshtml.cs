using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Library_System;
using Library_System.Models;
using Microsoft.AspNetCore.Authorization;

namespace Library_System.Pages.BorrowDetails
{
    [Authorize(Policy = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly Library_System.LibrarySystemContext _context;

        public DetailsModel(Library_System.LibrarySystemContext context)
        {
            _context = context;
        }

      public BorrowDetail BorrowDetail { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int id)
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
