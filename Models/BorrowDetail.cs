using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace Library_System.Models
{
    public class BorrowDetail
    {
        [Key]
        public int BorrowId { get; set; }
        public int AccountId { get; set; }
        public int BookId { get; set; }
        [DataType(DataType.Date)]
        public DateTime BorrowDate { get; set; }
        [DataType(DataType.Date)]
		// [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
		[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime ReturnDate { get; set; }
        public string Status { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DeleteAt { get; set; }
        [ValidateNever]
        public Book? Book { get; set; }
        [ValidateNever]
        public Account? Account { get; set; }
    }
}
