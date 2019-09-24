using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Models.Entities
{
   public class Message
    {
        public int MessageId { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public int DialogId { get; set; }
        public DateTime Date { get; set; }
        public virtual User User { get; set; }
        public virtual Dialog Dialog { get; set; }
        public virtual ICollection<File> Files { get; set; } = new List<File>();
    }
}
