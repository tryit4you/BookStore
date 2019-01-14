using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace BookStore.Services
{
    public  class EmailSender
    {
      
        public async static void SendMail(string email, string confirmationLink)
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
                Subject = "Xác thực email từ EbookViet.vn",
                Body = confirmationLink
            })
            {
                await smtpClient.SendMailAsync(mess);
            }
        }
    }
}