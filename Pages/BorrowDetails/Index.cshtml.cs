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
using System.Globalization;
using System.Text;
using Library_System.Utils;
using System.Drawing.Printing;

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
        //[BindProperty]
        //public IList<BorrowDetail> BorrowDetail { get; set; } = default!;
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
		[BindProperty]
		public PaginatedList<BorrowDetail> BorrowDetail { get; set; } = default!;

		public IQueryable<BorrowDetail> BorrowDetailsIQ { get; set; } = default!;
		public async Task OnGetAsync(int? pageIndex)
        {
			pageIndex = pageIndex ?? 1;
			await getDataStart(pageIndex);
			GetOptionFilter();
			await Filter();
			await Pagging(pageIndex);
		}
        public async Task getDataStart(int? pageIndex)
        {
			var account = HttpContext.User;
			// get username off account
			var userName = account.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			BorrowDetailsIQ = from s in _context.BorrowDetails
													   select s;

			if (_context.BorrowDetails != null)
			{
				if (User.Claims.SingleOrDefault(c => c.Type == "isAdmin")?.Value == "True")
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
			List<string> status = new List<string>() { "Booked", "Pending", "Borrowed", "OutOfDate", "Returned", "Canceled", };
			ViewData["Status"] = new SelectList(status);
		}
		public async Task Pagging(int? pageIndex)
		{
			var pageSize = Configuration.GetValue("PageSize", 3);
			BorrowDetailsIQ = BorrowDetailsIQ.AsNoTracking();
			BorrowDetail = await PaginatedList<BorrowDetail>.CreateAsync(
				BorrowDetailsIQ.AsNoTracking(),
				pageIndex ?? 1, pageSize);
		}
        public void GetOptionFilter()
        {
            status = status == null ?"All":status;
            option = option == 0 ? 1 : option;
            search = search == null ? "" : search;
            optionDate = optionDate == 0 ? 1 : optionDate;
			startDate = startDate == default ? default! : startDate;
            endDate = endDate == default ? DateTime.Now : endDate;

        }
		public async Task OnPostSearch(int? pageIndex)
		{
			await OnGetAsync(pageIndex);
		}

		public async Task Filter()
		{
			search = RemoveDiacritics(search).ToLower().Trim();

			if (status != "All")
			{
				BorrowDetailsIQ = BorrowDetailsIQ.Where(o => o.Status == status);
			}
			if (option == 1)
			{
				BorrowDetailsIQ = BorrowDetailsIQ.Where(o => o.Account.UserName.ToLower().Contains(search));
			}
			else if (option == 2)
			{
				BorrowDetailsIQ = BorrowDetailsIQ.Include(b => b.Book)
					.Include(b => b.Account)
					.Where(o => RemoveDiacritics(o.Book.BookName).ToLower().Trim().Contains(search));
			}
			if (optionDate == 1)
			{
				BorrowDetailsIQ = BorrowDetailsIQ.Where(o => o.BorrowDate.Date >= startDate.Date && o.BorrowDate.Date <= endDate.Date);
			}
			else if (optionDate == 2)
			{
				BorrowDetailsIQ = BorrowDetailsIQ.Where(o => o.ReturnDate.Date >= startDate.Date && o.ReturnDate.Date <= endDate.Date);
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
