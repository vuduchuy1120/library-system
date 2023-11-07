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

namespace Library_System.Pages.Authors
{
    [Authorize(Policy = "Admin")]

    public class IndexModel : PageModel
    {
        private readonly Library_System.LibrarySystemContext _context;

        public IndexModel(Library_System.LibrarySystemContext context)
        {
            _context = context;
        }

        public IList<Author> Author { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Authors != null)
            {
                Author = await _context.Authors.ToListAsync();
            }
        }
    }
}
