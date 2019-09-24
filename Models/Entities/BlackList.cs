using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Models.Entities
{
    public class BlackList
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ContactBlockId { get; set; }
        public virtual User User { get; set; }
        public virtual User ContactBlock { get; set; }
    }
}
