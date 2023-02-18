namespace TecnoSphere.Models.ContactEmailSender
{
    public interface IContactEmailSender
    {
        public void MessageSend(ContactMessage message);
    }
}
