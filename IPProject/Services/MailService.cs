using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace IPProject.Services
{
    public class MailService
    {
        public void SendEmail(string name, string email, string message)
        {
            System.Net.Mail.MailMessage objMailMessage = new System.Net.Mail.MailMessage();
            SmtpClient objSmtpClient = null;
            try
            {
                string from = ConfigurationManager.AppSettings["From"];
                string to = ConfigurationManager.AppSettings["To"];
                string pass = ConfigurationManager.AppSettings["Pass"];
                objMailMessage.From = new MailAddress(from);
                objMailMessage.To.Add(new MailAddress(to));
                objMailMessage.Subject = email;
                objMailMessage.Body = message;
                objMailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                objMailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                objSmtpClient = new SmtpClient("smtp.gmail.com", 587);
                objSmtpClient.UseDefaultCredentials = false;
                objSmtpClient.EnableSsl = true;
                objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                objSmtpClient.Credentials = new NetworkCredential(from, to);
                objSmtpClient.Send(objMailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objMailMessage = null;
                objSmtpClient = null;
            }
        }
    }
}