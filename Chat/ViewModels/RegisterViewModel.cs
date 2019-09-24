using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber  { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        //public string CurrentAvatar { get; set; }
        //public IFormFile Avatar { get; set; }
    }
}
