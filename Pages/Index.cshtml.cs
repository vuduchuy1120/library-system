using Library_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

namespace Library_System.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly Library_System.LibrarySystemContext _context;

        public IndexModel(ILogger<IndexModel> logger, LibrarySystemContext context)
        {
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public int categoryId { get; set; } 

        [BindProperty]
        public int option { get; set; } 
        [BindProperty]
        public string search { get; set; } 
        
        public List<Book> books = new List<Book>();
        public void OnGet()
        {
            categoryId = 0;
            option = 1;
            search = "";
            OnPost();


        }
        public void OnPost()
        {
            getBooks();
            getCategory();
            
        }
        public void getBooks()
        {
            books = _context.Books
                .Include(b => b.Author)
                .ToList();
            if (categoryId != 0)
            {
                books = books.Where(x => x.CategoryId == categoryId).ToList();
            }
            if (option == 1 || option == 2)
            {
                if (!String.IsNullOrEmpty(search))
                {
                    var normalizedSearch = RemoveDiacritics(search.ToLower()); 
                    if (option == 1)
                    {
                        books = books.Where(x => RemoveDiacritics(x.BookName.ToLower()).Contains(normalizedSearch)).ToList();
                    }
                    else if (option == 2)
                    {
                        books = books.Where(x => RemoveDiacritics(x.Author.AuthorName.ToLower()).Contains(normalizedSearch)).ToList();
                    }
                }
            }
        }

        public void getCategory()
        {
            var categoryList = new List<SelectListItem>();

            categoryList.Add(new SelectListItem
            {
                Text = "View all",
                Value = "0"
            });

            categoryList.AddRange(_context.Categories.Select(x =>
              new SelectListItem
              {
                  Text = x.CategoryName,
                  Value = x.Id.ToString()
              }
            ));
            ViewData["CategoryId"] = new SelectList(categoryList, "Value", "Text");
        }
        public static string RemoveDiacritics(string text)
        {
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
    }
}