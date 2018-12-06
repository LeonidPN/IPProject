using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace IPProject.Services
{
    public class MailService
    {
        public async Task SendEmail(string name, string email, string message)
        {
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(name, ConfigurationManager.AppSettings["From"]));
                emailMessage.To.Add(new MailboxAddress("", ConfigurationManager.AppSettings["To"]));
                emailMessage.Subject = email;
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text)
                {
                    Text = message
                };
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, false);
                    await client.AuthenticateAsync(ConfigurationManager.AppSettings["From"], ConfigurationManager.AppSettings["Pass"]);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}