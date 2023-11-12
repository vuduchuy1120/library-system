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
using Library_System.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Library_System.Pages.BorrowDetails
{
    [Authorize(Policy = "Admin")]
    public class EditModel : PageModel
    {
        private readonly Library_System.LibrarySystemContext _context;
        private readonly IHubContext<SignalrHub> _hubContext;

        public EditModel(Library_System.LibrarySystemContext context,IHubContext<SignalrHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
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
            List<string> status = new List<string>() { "Booked", "Borrowed", "OutOfDate", "Returned", "Canceled" };
            ViewData["Status"] = new SelectList(status);
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

            _hubContext.Clients.All.SendAsync("LoadEdit"
                ,BorrowDetail.BorrowId
                , _context.Accounts.Find(BorrowDetail.AccountId).UserName
                , _context.Books.Find(BorrowDetail.BookId).BookName
                , BorrowDetail.BorrowDate.ToString("MM/d/yyyy")
                , BorrowDetail.ReturnDate.ToString("MM/d/yyyy")
                , BorrowDetail.Status);
            return RedirectToPage("./Index");
        }

        private bool BorrowDetailExists(int id)
        {
          return (_context.BorrowDetails?.Any(e => e.BorrowId == id)).GetValueOrDefault();
        }
    }
}
