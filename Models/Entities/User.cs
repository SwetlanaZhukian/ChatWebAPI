using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Chat.Models.Entities
{
    public class User: IdentityUser
    {
        public string Name { get; set; }
        public virtual ICollection<BlackList> UserInBlackLists { get; set; }
        public virtual ICollection<BlackList> ContactBlackLists { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Contact> UserInContacts { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection <Dialog> UserDialogs { get; set; }
        public virtual  ICollection <Dialog> ContactDialogs { get; set; }
        public string Avatar { get; set; }
        public DateTime LastTimeOnline { get; set; }
        public User()
        {
            UserDialogs = new List<Dialog>();
            ContactDialogs = new List<Dialog>();
            UserInBlackLists = new List<BlackList>();
            ContactBlackLists = new List<BlackList>();
            Contacts = new List<Contact>();
            Messages = new List<Message>();
            UserInContacts = new List<Contact>();
           
        }
    }
}
