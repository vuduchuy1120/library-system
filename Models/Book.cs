using System;
using System.Collections.Generic;

namespace LibrarySystem.Models
{
    public partial class Book
    {
        public Book()
        {
            BorrowDetails = new HashSet<BorrowDetail>();
        }

        public int Id { get; set; }
        public string Isbncode { get; set; }
        public string BookName { get; set; }
        public int? CategoryId { get; set; }
        public int? AuthorId { get; set; }
        public int? PublisherId { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public DateTime? Year { get; set; }

        public virtual Author Author { get; set; }
        public virtual Category Category { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual ICollection<BorrowDetail> BorrowDetails { get; set; }
    }
}
