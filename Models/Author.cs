using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Library_System.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string? Bio { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DeleteAt { get; set; }
        [ValidateNever]
        public ICollection<Book>? Books { get; set; }
    }
}
