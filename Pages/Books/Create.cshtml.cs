﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Library_System;
using Library_System.Models;
using Microsoft.AspNetCore.Authorization;

namespace Library_System.Pages.Books
{
    [Authorize(Policy = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly Library_System.LibrarySystemContext _context;

        public CreateModel(Library_System.LibrarySystemContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "AuthorName");
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName");
        ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "PublisherName");
            return Page();
        }

        [BindProperty]
        public Book Book { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Books == null || Book == null)
            {
                return Page();
            }
            try
            {
                _context.Books.Add(Book);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("Book.ISBN", "ISBN already exists");   
                return Page();
            }

            
        }
    }
}
