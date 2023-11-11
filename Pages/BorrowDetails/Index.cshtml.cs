using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Library_System;
using Library_System.Models;

namespace Library_System.Pages.BorrowDetails
{
    public class IndexModel : PageModel
    {
        private readonly Library_System.LibrarySystemContext _context;

        public IndexModel(Library_System.LibrarySystemContext context)
        {
            _context = context;
        }

        public IList<BorrowDetail> BorrowDetail { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.BorrowDetails != null)
            {
                BorrowDetail = await _context.BorrowDetails
                .Include(b => b.Account)
                .Include(b => b.Book).ToListAsync();
            }
        }

        public IActionResult OnPostExtention(int id)
        {
            BorrowDetail borrowDetail = _context.BorrowDetails.Find(id);
            // Add 7 days to return date
            borrowDetail.ReturnDate = borrowDetail.ReturnDate.AddDays(7);
            _context.SaveChanges();
            return RedirectToPage("./Index");
        }
    }
}
