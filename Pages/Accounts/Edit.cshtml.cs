using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library_System;
using Library_System.Models;

namespace Library_System.Pages.Accounts
{
    public class EditModel : PageModel
    {
        private readonly Library_System.LibrarySystemContext _context;

        public EditModel(Library_System.LibrarySystemContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Account Account { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account =  await _context.Accounts.FirstOrDefaultAsync(m => m.UserId == id);

            if (account == null)
            {
                return NotFound();
            }
            Account = account;

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Account acc =Account;
                return Page();
            }


            _context.Attach(Account).State = EntityState.Modified;
            string email = Account.Email;
            //_context.Entry(Account).Property(a => a.UserName).IsModified = false;
            //_context.Entry(Account).Property(a => a.Email).IsModified = false;
            //_context.Entry(Account).Property(a => a.Password).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(Account.UserId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AccountExists(int id)
        {
          return (_context.Accounts?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
        // check email, username, phone number
        private bool AccountExists(int id,string email, string username, string phoneNumber)
        {
          return (_context.Accounts?.Any(e => e.UserId == id|| e.Email == email || e.UserName == username || e.Phone == phoneNumber)).GetValueOrDefault();
        }
    }
}
