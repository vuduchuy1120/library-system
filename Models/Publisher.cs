using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Library_System.Models
{
    public class Publisher
    {
        [Key]
        public int Id { get; set; }
        public string PublisherName { get; set; }
        public string? Address { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DeleteAt { get; set; }
        [ValidateNever]
        public ICollection<Book> Books { get; set; }
    }
}
