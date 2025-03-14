using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace TestIdentity.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("mohamedashrafmahmoudgad@gmail.com", "wlbu yymj qybp xeby")
            };

            return client.SendMailAsync(
                new MailMessage(from: "mohamedashrafmahmoudgad@gmail.com",
                                to: email,
                                subject,
                                message
                                )
                {
                    IsBodyHtml = true
                });
        }
    }
}
