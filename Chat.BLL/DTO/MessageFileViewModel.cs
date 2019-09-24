using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.ViewModels
{
    public class MessageFileViewModel
    {
        public string Text { get; set; }
        public string receiverId { get; set; }
        public IFormFileCollection Files { get; set; }
    }
}
