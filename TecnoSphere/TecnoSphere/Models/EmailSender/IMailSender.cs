namespace TecnoSphere.Models.EmailSender
{
    public interface IMailSender
    {
        public void MessageSend(Message message);
    }
}
