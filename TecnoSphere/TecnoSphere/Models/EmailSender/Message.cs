namespace TecnoSphere.Models.EmailSender
{
    public class Message
    {
        public string Messageto { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public Message(string Messageto, string subject, string content)
        {
            this.Messageto = Messageto;
            Subject = subject;
            Content = content;
        }
    }
}
