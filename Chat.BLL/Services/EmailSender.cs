using Chat.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using Chat.BLL.Infrastructure;
using Microsoft.Extensions.Options;

namespace Chat.BLL.Services
{
   public class EmailSender:IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message, IOptions<EmailConfig> option)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", option.Value.Email));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await client.ConnectAsync("smtp.mail.ru", 587, MailKit.Security.SecureSocketOptions.Auto);
                //await client.ConnectAsync("smtp.mail.ru", 587, false);
                await client.AuthenticateAsync(option.Value.Email, option.Value.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
