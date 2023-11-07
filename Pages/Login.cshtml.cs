using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using Library_System.Models;

namespace Library_System.Pages
{
	public class User
	{
		[Required(ErrorMessage = "Username must be input value!")]
		public string Username { get; set; }
		[Required(ErrorMessage = "Password must be input value!")]
		public string Password { get; set; }

	}
	public class LoginModel : PageModel
	{
		private readonly LibrarySystemContext _context;


		public LoginModel(LibrarySystemContext context)
		{
			_context = context;
		}

		[BindProperty]
		public User User { get; set; }
		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid)
			{
				Account? customer = _context.Accounts.SingleOrDefault(
					x => x.UserName == User.Username && x.Password == User.Password
					);
				if (customer == null)
				{
					ModelState.AddModelError("Error", "Wrong username or password.");
					return Page();
				}
				else
				{
					var claims = new List<Claim>()
					{
						new Claim(ClaimTypes.NameIdentifier, customer.UserName),
						new Claim("isAdmin", customer.IsAdmin.ToString())
					};

					var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

					var authProperties = new AuthenticationProperties
					{
						AllowRefresh = true,
						IsPersistent = true
					};

					await HttpContext.SignInAsync(
							CookieAuthenticationDefaults.AuthenticationScheme,
							new ClaimsPrincipal(claimsIdentity),
							authProperties
							);

					return RedirectToPage("./Index");
				}
			}
			return Page();
		}
	}
}
