using Chat.BLL.Infrastructure;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.BLL.Interfaces
{
   public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, IOptions<EmailConfig> option);
    }
}
