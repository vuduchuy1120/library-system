using Library_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Library_System.Pages
{
    public class CartModel : PageModel
    {
        LibrarySystemContext _context;
        public CartModel(LibrarySystemContext context)
        {
            _context = context;
        }
        public List<Book> books;
        public string total;
        public void OnGet()
        {
            
            if(HttpContext.Session.GetString("books")!=null)
            {
                books = JsonSerializer.Deserialize<List<Book>>(HttpContext.Session.GetString("books"));
            }
            else
            {
                books = new List<Book>();
            }
            books = books.Where(books => books.UnitInStock > 0).ToList();
            total = HttpContext.Session.GetString("total");
        }

        public void OnPost(int id)
        {
            books = JsonSerializer.Deserialize<List<Book>>(HttpContext.Session.GetString("books"));
            Book book = books.Find(b => b.Id == id);
            books.Remove(book);
            HttpContext.Session.SetString("books", JsonSerializer.Serialize(books));
            HttpContext.Session.SetString("total", books.Count.ToString());


        }

        public IActionResult OnPostCheckout()
        {
            books = JsonSerializer.Deserialize<List<Book>>(HttpContext.Session.GetString("books"));
            foreach (var book in books)
            {
                
                BorrowDetail borrowDetail = new BorrowDetail()
                {
                    //AccountId = int.Parse(HttpContext.Session.GetString("id")),
                    AccountId = 1,
                    BookId = book.Id,
                    BorrowDate = DateTime.Now,
                    ReturnDate = DateTime.Now.AddDays(7),
                    Status = "Booked"
                };
                _context.BorrowDetails.Add(borrowDetail);
                _context.Books.Find(book.Id).UnitInStock--;
            }

            _context.SaveChanges();
            HttpContext.Session.Remove("books");
            HttpContext.Session.Remove("total");
            return RedirectToPage("./Index");
        }
    }
}
