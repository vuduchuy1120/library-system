using Library_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using System.Text.Json;

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
        [BindProperty]
        public bool showAlert { get; set; }
        [BindProperty]
        public string alertMessage { get; set; }
        
        public List<Book> books = new List<Book>();
        public async Task OnGet()
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

        public async Task<IActionResult> OnPostAddCart(int id)
        {
            Book book = _context.Books.Find(id);

            List<Book> books;
            if (HttpContext.Session.GetString("books")==null)
            {
                books = new List<Book>();
                books.Add(book);
                HttpContext.Session.SetString("books", JsonSerializer.Serialize(books));
            }
            else
            {
                books = JsonSerializer.Deserialize<List<Book>>(HttpContext.Session.GetString("books"));
                foreach (var item in books)
                {
                    if (item.Id == book.Id)
                    {
                        showAlert = true;
                        alertMessage = "This Book already exists in cart";
                        await OnGet();
                        return Page();
                    }
                }
                books.Add(book);
                HttpContext.Session.SetString("books", JsonSerializer.Serialize(books));
            }

            HttpContext.Session.SetString("total", books.Count.ToString());
            return RedirectToPage("./Index");
        }
        public void getBooks()
        {
            books = _context.Books
                .Include(b => b.Author)
                .Include(p=>p.Publisher)
                .Include(c=>c.Category)     
                .Where(b=>b.Author.DeleteAt==null && b.Publisher.DeleteAt == null && b.Category.DeleteAt == null && b.DeleteAt == null )
                .Where(b=>b.UnitInStock>0)
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

            categoryList.AddRange(_context.Categories
                .Where(c=>c.DeleteAt==null)
                .Select(x =>
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