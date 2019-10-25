using System;
using System.Collections.Generic;
using System.Text;

namespace EkoShop.Models.ViewModels
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public string Role { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }
    }
}
