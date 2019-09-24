using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Models.Entities
{
   public class Dialog
    {
        public int DialogId { get; set; }
        public string UserId { get; set; }
        public string ContactId { get; set; }

        public virtual User User { get; set; }
        public virtual User Contact { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
       
        public Dialog()
        {
            Messages = new List<Message>();
            
        }
    }
}
