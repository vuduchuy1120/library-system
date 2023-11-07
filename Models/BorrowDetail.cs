﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace Library_System.Models
{
    public class BorrowDetail
    {
        [Key]
        public string BorrowId { get; set; }
        public int AccountId { get; set; }
        public int BookId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Status { get; set; }
        [ValidateNever]
        public Book Book { get; set; }
        [ValidateNever]
        public Account Account { get; set; }
    }
}
