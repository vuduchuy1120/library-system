using System;
using System.Collections.Generic;

namespace LibrarySystem.Models
{
    public partial class BorrowDetail
    {
        public string BorrowId { get; set; }
        public int? UserId { get; set; }
        public int? BookId { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }

        public virtual Book Book { get; set; }
        public virtual Account User { get; set; }
    }
}
