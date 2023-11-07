using Library_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Library_System.Pages
{
   
    public class RegisterModel : PageModel
    {

        private readonly LibrarySystemContext _context;
        public RegisterModel(LibrarySystemContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Account Account { get; set; }

        [BindProperty]
        public string repassword { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            
            if (ModelState.IsValid)
            {             
                _context.Accounts.Add(Account);
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Index");
                }
                catch (DbUpdateException ex)
                {
                    if (!isUniqueEmail(Account.Email))
                    {
                        ModelState.AddModelError("Account.Email", "Email already exists");
                    }
                    if (!isUniqueUsername(Account.UserName))
                    {
                        ModelState.AddModelError("Account.UserName", "Username already exists");
                    }
                    if (!isUniquePhone(Account.Phone))
                    {
                        ModelState.AddModelError("Account.Phone", "Phone already exists");
                    }
                    if (!isRepasswordEqualPassword(Account.Password, repassword))
                    {
                        ModelState.AddModelError("repassword", "Repassword must be equal password");
                    }
                    return Page();
                }
            }
            return Page();
        }
        public bool isUniqueUsername(string username)
        {
            var account = _context.Accounts.FirstOrDefault(x => x.UserName == username);
            if (account == null)
            {
                return true;
            }
            return false;
        }
        public bool isUniqueEmail(string email)
        {
            var account = _context.Accounts.FirstOrDefault(x => x.Email == email);
            if (account == null)
            {
                return true;
            }
            return false;
        }
        public bool isUniquePhone(string phone)
        {
            var account = _context.Accounts.FirstOrDefault(x => x.Phone == phone);
            if (account == null)
            {
                return true;
            }
            return false;
        }
        // check repassword must be equal password
        public bool isRepasswordEqualPassword(string password, string repassword)
        {
            if (password.Equals(repassword))
            {
                return true;
            }
            return false;
        }


    }
}
