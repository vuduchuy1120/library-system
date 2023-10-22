﻿using System;
using System.Collections.Generic;

namespace LibrarySystem.Models
{
    public partial class Account
    {
        public Account()
        {
            BorrowDetails = new HashSet<BorrowDetail>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool? Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool? IsAdmin { get; set; }

        public virtual ICollection<BorrowDetail> BorrowDetails { get; set; }
    }
}
