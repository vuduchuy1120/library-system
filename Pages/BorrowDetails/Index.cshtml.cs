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
using System.Security.Claims;
using System.Globalization;
using System.Text;

namespace Library_System.Pages.BorrowDetails
{
    [Authorize]
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
        [BindProperty]
        public string status { get; set; } = default!;
		[BindProperty]
        public string search { get; set; } = default!;
		[BindProperty]
        public int option { get; set; } = default!;
		[BindProperty]
		public int optionDate { get; set; } = default!;
		[BindProperty]
        public DateTime startDate { get; set; } = default!;
		[BindProperty]
        public DateTime endDate { get; set; } = default!;

		public async Task OnGetAsync()
        {
			await getDataStart();
			setStartOptionFilter();

		}
        public async Task getDataStart()
        {
			var account = HttpContext.User;
			// get username off account
			var userName = account.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (_context.BorrowDetails != null)
			{
				if (User.Claims.SingleOrDefault(c => c.Type == "isAdmin")?.Value == "True")
				{
					BorrowDetail = await _context.BorrowDetails
					.Include(b => b.Account)
					.Include(b => b.Book)
					.ToListAsync();
				}
				else
				{
					BorrowDetail = await _context.BorrowDetails
					.Include(b => b.Account)
					.Include(b => b.Book)
					.Where(o => o.Account.UserName == userName)
					.ToListAsync();
				}


				foreach (var item in BorrowDetail.Where(item => item.ReturnDate <= DateTime.Now && item.Status == "Borrowed"))
				{
					item.Status = "OutOfDate";
					_context.BorrowDetails.Update(item);
					await _context.SaveChangesAsync();

				}

			}
			List<string> status = new List<string>() { "Booked", "Pending", "Borrowed", "OutOfDate", "Returned", "Canceled", };
			ViewData["Status"] = new SelectList(status);
		}
        public void setStartOptionFilter()
        {
            status = "All";
            option = 1;
            search = "";
            optionDate = 1;
            startDate = default!;
            endDate = DateTime.Now;

        }
        public async Task OnPostSearch()
        {
			await getDataStart();
			search = RemoveDiacritics(search).ToLower().Trim();
            
            if(status != "All")
            {
				BorrowDetail = BorrowDetail.Where(o => o.Status == status).ToList();
			}
            if (option == 1)
            {
                BorrowDetail = BorrowDetail.Where(o => o.Account.UserName.ToLower().Contains(search)).ToList();
            }
            else if(option ==2)
            {
                BorrowDetail = BorrowDetail.Where(o => RemoveDiacritics(o.Book.BookName.ToLower().Trim()).Contains(search)).ToList();
            }
			if(optionDate == 1)
			{
				BorrowDetail = BorrowDetail.Where(o => o.BorrowDate.Date >= startDate.Date && o.BorrowDate.Date <= endDate.Date).ToList();
			}
			else if(optionDate == 2)
			{
				BorrowDetail = BorrowDetail.Where(o => o.ReturnDate.Date >= startDate.Date && o.ReturnDate.Date <= endDate.Date).ToList();
			}
            
			
		}
		public static string RemoveDiacritics(string text)
		{
            text = text == null ? "" : text;
			var normalizedString = text.Normalize(NormalizationForm.FormD);
			var stringBuilder = new StringBuilder();

			foreach (var c in normalizedString)
			{
				var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
				if (unicodeCategory != UnicodeCategory.NonSpacingMark)
				{
					stringBuilder.Append(c);
				}
			}

			return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
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
				    _hubContext.Clients.All.SendAsync("LoadReturnDate", borrowDetail.BorrowId, borrowDetail.ReturnDate.ToString("M/d/yyyy"));
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
