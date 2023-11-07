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
            books = JsonSerializer.Deserialize<List<Book>>(HttpContext.Session.GetString("books"));
            if(books == null)
            {
                books = new List<Book>();
            }
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
    }
}
