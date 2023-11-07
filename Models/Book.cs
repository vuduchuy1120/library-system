using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Library_System.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string IsbCode { get; set; }
        public string BookName { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitInStock { get; set; }
        public DateTime Year { get; set; }
        public string Image { get; set; }

        [ForeignKey("AuthorId")]
        [ValidateNever]
        public Author Author { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }
        [ForeignKey("PublisherId")]
        [ValidateNever]
        public Publisher Publisher { get; set; }
        [ValidateNever]
        public ICollection<BorrowDetail> BorrowDetails { get; set; }

    }
}
