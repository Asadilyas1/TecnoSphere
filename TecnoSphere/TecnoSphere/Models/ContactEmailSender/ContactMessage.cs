namespace TecnoSphere.Models.ContactEmailSender
{
    public class ContactMessage
    {
        public string Messageto { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public ContactMessage(string Messageto, string subject, string content)
        {
            this.Messageto = Messageto;
            Subject = subject;
            Content = content;
        }
    }
}
