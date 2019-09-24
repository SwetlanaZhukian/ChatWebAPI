using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Models.Entities
{
   public class File
    {
        public int FileId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int MessageId { get; set; }
        public virtual Message Message { get; set; }
    }
}
