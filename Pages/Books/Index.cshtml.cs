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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library_System.Pages.Books
{
    [Authorize(Policy = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly Library_System.LibrarySystemContext _context;

        public IndexModel(Library_System.LibrarySystemContext context)
        {
            _context = context;
        }
		[BindProperty]
		public int categoryId { get; set; }

		[BindProperty]
		public int option { get; set; }
		[BindProperty]
		public string search { get; set; }
		public IList<Book> books { get;set; } = default!;

        public async Task OnGetAsync()
        {
			getCategory();
            GetOptionFillter();
			await getBooks();

        }

        public async Task OnPostAsync()
        {
            await OnGetAsync();
        }

		public async Task getBooks()
        {
            books = await _context.Books
                .Include(b => b.Author)
                .Include(p=>p.Publisher)
                .Include(c=>c.Category)     
                .Where(b=>b.Author.DeleteAt==null && b.Publisher.DeleteAt == null && b.Category.DeleteAt == null && b.DeleteAt == null )
                .Where(b=>b.UnitInStock>0)
                .ToListAsync();
            if (categoryId != 0)
            {
                books = books.Where(x => x.CategoryId == categoryId).ToList();
            }
            if (option == 1 || option == 2 || option ==3)
            {
                if (!String.IsNullOrEmpty(search))
                {
                    var normalizedSearch = search.ToLower().Trim(); 
                    if (option == 1)
                    {
                        books = books.Where(x => x.BookName.ToLower().Contains(normalizedSearch)).ToList();
                    }
                    else if (option == 2)
                    {
                        books = books.Where(x =>x.Author.AuthorName.ToLower().Contains(normalizedSearch)).ToList();
                    }
                    else if (option == 3)
                    {
						books = books.Where(x => x.IsbCode.ToLower().Contains(normalizedSearch)).ToList();
					}
                }
            }
        }
        public void GetOptionFillter()
        {
			option = option == 0 ? 1 : option;
            search = search == null ? "" : search;
        }
		public void getCategory()
		{
			var categoryList = new List<SelectListItem>();

			categoryList.Add(new SelectListItem
			{
				Text = "View all",
				Value = "0"
			});

			categoryList.AddRange(_context.Categories
				.Where(c => c.DeleteAt == null)
				.Select(x =>
			  new SelectListItem
			  {
				  Text = x.CategoryName,
				  Value = x.Id.ToString()
			  }
			));
			ViewData["CategoryId"] = new SelectList(categoryList, "Value", "Text");
		}
	}
}
