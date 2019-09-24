﻿using Chat.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.BLL.DTO
{
    public class DialogViewModel
    {
        public int DialogId { get; set; }
        public string LastMessage { get; set; }
        public string UserId { get; set; }
        public string ContactId { get; set; }
        public string Avatar { get; set; }
        public string ContactName { get; set; }
        public string Date { get; set; }
        public bool InBlock { get; set; }
        public List<MessageViewModel> Messages{get;set;}
      

        public DialogViewModel(Dialog dialog, User user)
        {
            DialogId = dialog.DialogId;
            if (user.Id==dialog.UserId)
            {
                UserId = dialog.UserId;
                ContactId = dialog.ContactId;
                Avatar = dialog.Contact.Avatar;
                InBlock = false;
                if (Avatar==null)
                {
                    Avatar= "\\Resources\\Images\\default.jpg";
                }
                ContactName = dialog.Contact.Name;
                var lastMessage = dialog.Messages.LastOrDefault();
                if (dialog.Messages.Count != 0)
                {
                    LastMessage = lastMessage.Content;
                    Date = lastMessage.Date.ToString("dd-MM-yyyy HH:mm");
                   
                }
            }
            if (user.Id == dialog.ContactId)
            {
                UserId = dialog.ContactId;
                ContactId = dialog.UserId;
                Avatar = dialog.User.Avatar;
                InBlock = false;
                if (Avatar == null)
                {
                    Avatar = "\\Resources\\Images\\default.jpg";
                }
                ContactName = dialog.User.Name;
                var lastMessage = dialog.Messages.LastOrDefault();
                if (dialog.Messages.Count != 0)
                {
                    LastMessage = lastMessage.Content;
                    Date = lastMessage.Date.ToString("dd-MM-yyyy HH:mm");
                   
                }
            }
        }
    }
}
