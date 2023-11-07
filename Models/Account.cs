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
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set;}
        public bool IsAdmin { get; set; }
        [ValidateNever]
        public ICollection<BorrowDetail> BorrowDetails { get; set; }
    }
}
