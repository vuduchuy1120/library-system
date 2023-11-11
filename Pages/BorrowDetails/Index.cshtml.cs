﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Library_System;
using Library_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Library_System.Hubs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library_System.Pages.BorrowDetails
{
    [Authorize(Policy = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly Library_System.LibrarySystemContext _context;
        private readonly IHubContext<SignalrHub> _hubContext;
        public IndexModel(Library_System.LibrarySystemContext context, IHubContext<SignalrHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        [BindProperty]
        public IList<BorrowDetail> BorrowDetail { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.BorrowDetails != null)
            {
                BorrowDetail = await _context.BorrowDetails
                .Include(b => b.Account)
                .Include(b => b.Book).ToListAsync();

                foreach (var item in BorrowDetail.Where(item => item.ReturnDate <= DateTime.Now && item.Status == "Borrowed"))
                {
                    item.Status = "OutOfDate";
                    _context.BorrowDetails.Update(item);
                    await _context.SaveChangesAsync();
                    
                }

            }
            List<string> status = new List<string>() { "Booked", "Borrowed", "OutOfDate", "Returned" };
            ViewData["Status"] = new SelectList(status);

        }

    }
}
