using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Library_System.Models
{
    public class Account
    {
        [Key]
        public int UserId { get; set; }
        //only word and number
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Username not contains special characters!")]
        public string UserName { get; set; }
        // at least 8 characters, at least one uppercase letter, one lowercase letter and one number
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", ErrorMessage = "Password must be at least 8 characters, at least one uppercase letter, one lowercase letter and one number.")]
        public string Password { get; set; }
        public bool Gender { get; set; } = true;
        [DataType(DataType.EmailAddress)]
        // email must be input
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\d{9,10}$", ErrorMessage = "Phone number must be 9 or 10 digits.")]
        public string Phone { get; set; }
        public string? Address { get; set;}
        public bool IsAdmin { get; set; } = false;
        [DataType(DataType.Date)]
        public DateTime? DeleteAt { get; set; }
        [ValidateNever]
        public ICollection<BorrowDetail> BorrowDetails { get; set; }
    }
}
