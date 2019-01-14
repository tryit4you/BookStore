using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BookStore.Areas.Admin.Services
{
    public class EmailSender
    {
        public async static void SendMail(string email, string message,string subject)
        {

            var smtpClient = new SmtpClient
            {
                Host = "smtp.gmail.com", // set your SMTP server name here
                Port = 587, // Port
                EnableSsl = true,
                Credentials = new NetworkCredential("vohung.it@gmail.com", "hung!@1997")
            };

            using (var mess = new MailMessage("vohung.it@gmail.com", email)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml=true
            })
            {
                await smtpClient.SendMailAsync(mess);
            }
        }
    }
}
