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
    public class IndexModel : PageModel
    {
        private readonly LibrarySystem.Models.LibrarySystemContext _context;

        public IndexModel(LibrarySystem.Models.LibrarySystemContext context)
        {
            _context = context;
        }

        public IList<BorrowDetail> BorrowDetail { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.BorrowDetails != null)
            {
                BorrowDetail = await _context.BorrowDetails
                .Include(b => b.Book)
                .Include(b => b.User).ToListAsync();
            }
        }
    }
}
