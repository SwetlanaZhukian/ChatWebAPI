using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Chat.Models.Entities
{
   public class Contact
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FriendId { get; set; }
        public virtual User User { get; set; }
        public virtual User Friend{ get; set; }
        public string  NickName { get; set; }
      
    }
}
