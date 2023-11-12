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
        [Display(Name = "ISBN")]
        public string IsbCode { get; set; }
        [Display(Name = "Name")]
        public string BookName { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
        public string? QuantityPerUnit { get; set; } = "1";
        public decimal? UnitPrice { get; set; }
        public int? UnitInStock { get; set; } = 1;
        [DataType(DataType.Date)]
        public DateTime? Year { get; set; }
        public string Image { get; set; }
        public string? Description { get; set; }
        public String? Language { get; set; }
        public int? PrintLength { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DeleteAt { get; set; }

        [ForeignKey("AuthorId")]
        [ValidateNever]
        public Author? Author { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category? Category { get; set; }
        [ForeignKey("PublisherId")]
        [ValidateNever]
        public Publisher? Publisher { get; set; }
        [ValidateNever]
        public ICollection<BorrowDetail> BorrowDetails { get; set; }

    }
}
