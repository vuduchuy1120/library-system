﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Library_System;
using Library_System.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Library_System.Hubs;

namespace Library_System.Pages.BorrowDetails
{
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
        public IList<BorrowDetail> BorrowDetail { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.BorrowDetails != null)
            {
                BorrowDetail = await _context.BorrowDetails
                .Include(b => b.Account)
                .Include(b => b.Book).ToListAsync();
            }
            List<string> status = new List<string>() {"Booked", "Borrowed", "OutOfDate", "Returned" };
            ViewData["Status"] = new SelectList(status);

        }

        public IActionResult OnPostExtention(int id)
        {
            BorrowDetail borrowDetail = _context.BorrowDetails.Find(id);
            // Add 7 days to return date
            borrowDetail.ReturnDate = borrowDetail.ReturnDate.AddDays(7);
            if(borrowDetail.ReturnDate > DateTime.Now)
            {
                borrowDetail.Status = "Borrowed";
            }
            _context.SaveChanges();
            
            return RedirectToPage("./Index");
        }

        public IActionResult OnPostChageStatus(int id, string status)
        {
            BorrowDetail borrowDetail = _context.BorrowDetails.Find(id);
            borrowDetail.Status = status;
            _context.SaveChanges();
            _hubContext.Clients.All.SendAsync("LoadStatus", borrowDetail.BorrowId, borrowDetail.Status);
            return RedirectToPage("./Index");
        }
    }
}
