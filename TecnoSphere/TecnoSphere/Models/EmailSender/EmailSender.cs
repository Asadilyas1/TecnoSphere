using System.Net.Mail;
using System.Net;
using MimeKit;
using System.Net.Mime;

namespace TecnoSphere.Models.EmailSender
{
        public class EmailSender : IMailSender
        {

        private readonly IWebHostEnvironment _hostEnvironment;
        public EmailSender(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public void MessageSend(Message message)
            {
                try
                {
                  var PathRoot=_hostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString() + "TecnoEmail" +
                    
                    Path.DirectorySeparatorChar.ToString() +"email.html";

                var builder = new BodyBuilder();
                using (StreamReader SourceReader = System.IO.File.OpenText(PathRoot))
                {
                    builder.HtmlBody = SourceReader.ReadToEnd();
                }
                string messageBody = string.Format(builder.HtmlBody);

                var mail = new MailMessage();
                    var client = new System.Net.Mail.SmtpClient("", 26) //Port 8025, 587 and 25 can also be used.
                    {
                        Credentials = new NetworkCredential("", ""),
                        EnableSsl = false,
                    };
                    mail.From = new MailAddress("", "");
                    mail.To.Add(message.Messageto);
                    mail.Subject = message.Subject;
                    var htmlView = AlternateView.CreateAlternateViewFromString(messageBody, null, MediaTypeNames.Text.Html);
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
