using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.ViewModels
{
    public class ContactViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Avatar { get; set; }
        public bool UserInContact { get; set; }
        public bool Status { get; set; }
        public bool IsOnline { get; set; }
        public string LastTimeOnline { get; set; }

    }
}
