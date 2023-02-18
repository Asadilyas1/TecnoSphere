using Microsoft.Extensions.Hosting;
using MimeKit;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;

namespace TecnoSphere.Models.ContactEmailSender
{
    public class ContactEmailSender : IContactEmailSender
    {
        public void MessageSend(ContactMessage message)
        {
            try
            {
               
                var mail = new MailMessage();
                var client = new System.Net.Mail.SmtpClient("", 26) //Port 8025, 587 and 25 can also be used.
                {
                    Credentials = new NetworkCredential("", ""),
                    EnableSsl = false,
                };
                mail.From = new MailAddress("", "");
                mail.To.Add(message.Messageto);
                mail.Subject = message.Subject;
                var htmlView = AlternateView.CreateAlternateViewFromString(message.Content, null, MediaTypeNames.Text.Html);
                mail.AlternateViews.Add(htmlView);
                client.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
