using Chat.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.BLL.DTO
{
   public class MessageViewModel
   {    
        public  int  MessageId { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public string Date { get; set; }
        public string Name { get; set; }
       
        public ICollection<FileViewModel> FilePaths { get; set; }

        public MessageViewModel(Message message)
        {
            if (message != null)
            {
                MessageId = message.MessageId;
                if (!String.IsNullOrEmpty(message.Content))
                {
                    Content = message.Content;
                } 
                UserId = message.UserId;
                Date=message.Date.ToString("dd-MM-yyyy HH:mm");
                Name = message.User.Name;
                if (message.Files != null)
                {
                    var result = new List<FileViewModel>();
                    foreach (var file in message.Files)
                    {
                        result.Add(new FileViewModel(file));
                    }
                    FilePaths = result;
                }
                

            }

        }
       

    }           

   
}
