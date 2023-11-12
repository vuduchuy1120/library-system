using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Library_System;
using Library_System.Models;

namespace Library_System.Pages.Accounts
{
    public class CreateModel : PageModel
    {
        private readonly Library_System.LibrarySystemContext _context;

        public CreateModel(Library_System.LibrarySystemContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Account Account { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid || _context.Accounts == null || Account == null)
                {
                    return Page();
                }

                _context.Accounts.Add(Account);
                await _context.SaveChangesAsync();
            }catch(Exception e)
            {
                if (!isUniqueEmail(Account.Email))
                {
                    ModelState.AddModelError("Account.Email", "Email already exists");

                }
                if(!isUniqueUsername(Account.UserName))
                {
                    ModelState.AddModelError("Account.UserName", "Username already exists");
                }
                if(!isUniquePhone(Account.Phone))
                {
                    ModelState.AddModelError("Account.Phone", "Phone already exists");
                }
                return Page();
            }
         

            return RedirectToPage("./Index");
        }

        // check isUnique for Email
        public bool isUniqueEmail(string email)
        {
            if (_context.Accounts == null)
            {
                return true;
            }
            foreach (var account in _context.Accounts)
            {
                if (account.Email == email)
                {
                    return false;
                }
            }
            return true;
        }
        // check isUnique for Username
        public bool isUniqueUsername(string username)
        {
            if (_context.Accounts == null)
            {
                return true;
            }
            foreach (var account in _context.Accounts)
            {
                if (account.UserName.Equals(username))
                {
                    return false;
                }
            }
            return true;
        }
        // check isUnique for Phone
        public bool isUniquePhone(string phone)
        {
            if (_context.Accounts == null)
            {
                return true;
            }
            foreach (var account in _context.Accounts)
            {
                if (account.Phone == phone)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
