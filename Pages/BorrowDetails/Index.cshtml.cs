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
using Microsoft.AspNetCore.SignalR;
using Library_System.Hubs;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Library_System.Utils;

namespace Library_System.Pages.BorrowDetails
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly Library_System.LibrarySystemContext _context;
        private readonly IHubContext<SignalrHub> _hubContext;
		private readonly IConfiguration Configuration;

		public IndexModel(Library_System.LibrarySystemContext context, IHubContext<SignalrHub> hubContext, IConfiguration configuration)
        {
            _context = context;
            _hubContext = hubContext;
			Configuration = configuration;
		}

		[BindProperty]
        public PaginatedList<BorrowDetail> BorrowDetail { get; set; } = default!;

        public async Task OnGetAsync(int? pageIndex)
        {
			pageIndex = pageIndex ?? 1; 
			var account = HttpContext.User;
            // get username off account
            var userName = account.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            IQueryable<BorrowDetail> BorrowDetailsIQ = from s in _context.BorrowDetails													   
													   select s;

			if (_context.BorrowDetails != null)
            {
                if(User.Claims.SingleOrDefault(c => c.Type == "isAdmin")?.Value == "True")
                {
					BorrowDetailsIQ = from s in _context.BorrowDetails						 
									  select s;
					BorrowDetailsIQ = BorrowDetailsIQ.Include(b => b.Book).Include(b => b.Account);

				}
                else
                {
					BorrowDetailsIQ = from s in _context.BorrowDetails                                      
                                      where s.Account.UserName == userName
                                      select s;
					BorrowDetailsIQ = BorrowDetailsIQ.Include(b => b.Book).Include(b => b.Account);

				}
				foreach (var item in BorrowDetailsIQ.Where(item => item.ReturnDate <= DateTime.Now && item.Status == "Borrowed"))
				{
					item.Status = "OutOfDate";
					_context.BorrowDetails.Update(item);
					await _context.SaveChangesAsync();
					_hubContext.Clients.All.SendAsync("LoadStatus", item.BorrowId, item.Status);
					_hubContext.Clients.All.SendAsync("LoadReturnDate", item.BorrowId, item.ReturnDate.ToString("MM/dd/yyyy"));

				}
			}
			List<string> status = new List<string>() {"Booked", "Pending", "Borrowed", "OutOfDate", "Returned", "Canceled", };
            ViewData["Status"] = new SelectList(status);
			var pageSize = Configuration.GetValue("PageSize", 10);
			BorrowDetailsIQ = BorrowDetailsIQ.AsNoTracking(); 

			BorrowDetail = await PaginatedList<BorrowDetail>.CreateAsync(
				BorrowDetailsIQ.AsNoTracking(),
				pageIndex ?? 1, pageSize);
		}


        public IActionResult OnPostExtention(int? id)
        {
            var borrowDetail = _context.BorrowDetails.Where(c=> c.BorrowId == id).SingleOrDefault();
            // Add 7 days to return date
            if(User.Claims.SingleOrDefault(c => c.Type == "isAdmin")?.Value == "False")
            {
                if(borrowDetail.Status == "Borrowed")
                {
                    borrowDetail.Status = "Pending";
					TempData["SuccessMessage"] = "The request extends successfully! Please wait a moment.";
					_context.SaveChanges();
				    _hubContext.Clients.All.SendAsync("LoadReturnDate", borrowDetail.BorrowId, borrowDetail.ReturnDate.ToString("MM/dd/yyyy"));
                }
                else
                {
                    TempData["ErrorMessage"] = "You cannot extend the book return date. Please contact the librarian to resolve the issue.";
                }
			}
            return RedirectToPage("./Index");
        }


        public IActionResult OnPostChageStatus(int id, string status)
        {
            BorrowDetail borrowDetail = _context.BorrowDetails.Find(id);
            if(borrowDetail.Status == "Pending" && status == "Borrowed")
            {
				borrowDetail.ReturnDate = borrowDetail.ReturnDate.AddDays(7);
				if (borrowDetail.ReturnDate > DateTime.Now)
				{
					borrowDetail.Status = "Borrowed";
				}
				_context.SaveChanges();
				_hubContext.Clients.All.SendAsync("LoadReturnDate", borrowDetail.BorrowId, borrowDetail.ReturnDate.ToString("M/d/yyyy"));
			}
            else
            {
				borrowDetail.Status = status;
				_context.SaveChanges();
				_hubContext.Clients.All.SendAsync("LoadStatus", borrowDetail.BorrowId, borrowDetail.Status);
			}
          
            return RedirectToPage("./Index");
        }
    }
}
