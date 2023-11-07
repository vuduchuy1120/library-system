using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Library_System.Models
{
    public class Account
    {
        [Key]
        public int UserId { get; set; }
        
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public bool Gender { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\d{9,10}$", ErrorMessage = "Phone number must be 9 or 10 digits.")]
        public string Phone { get; set; }
        public string? Address { get; set;}
        public bool IsAdmin { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DeleteAt { get; set; }
        [ValidateNever]
        public ICollection<BorrowDetail> BorrowDetails { get; set; }
    }
}
