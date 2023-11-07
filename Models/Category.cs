using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Library_System.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string? Descriptions { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DeleteAt { get; set; }
        [ValidateNever]
        public ICollection<Book> Books { get; set; }
    }
}
