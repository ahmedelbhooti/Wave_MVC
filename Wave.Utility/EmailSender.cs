using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Wave.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var emailSettings = _configuration.GetSection("EmailSettings");
                var smtpServer = emailSettings["SmtpServer"];
                var smtpPort = int.Parse(emailSettings["SmtpPort"]);
                var smtpUserName = emailSettings["UserName"];
                var smtpPassword = emailSettings["Password"];
                var smtpTimeout = int.Parse(emailSettings["SmtpTimeout"]);

                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(smtpUserName);
                    message.To.Add(new MailAddress(email));
                    message.Subject = subject;
                    message.Body = htmlMessage;
                    message.IsBodyHtml = true;

                    using (var client = new SmtpClient(smtpServer, smtpPort))
                    {
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential(smtpUserName, smtpPassword);
                        client.EnableSsl = true;
                        client.Timeout = smtpTimeout;

                        await client.SendMailAsync(message);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately (e.g., logging, error handling)
                // You can rethrow the exception or return an error message if needed
                throw;
            }
        }
    }
}

