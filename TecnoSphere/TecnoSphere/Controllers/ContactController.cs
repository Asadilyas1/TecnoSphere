using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using TecnoSphere.Areas.Identity.Data;
using TecnoSphere.Models.ContactEmailSender;
using TecnoSphere.Models.EmailSender;

namespace TecnoSphere.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDBContext _context;
        public readonly IContactEmailSender _sender;
        public ContactController(ApplicationDBContext context,IContactEmailSender sender)
        {
            _context = context;
            _sender = sender;   
        }

        public IActionResult ContactList()
        {
            var Contact = _context.ContactUs.Include(x => x.services).Where(x=>x.MessagesStatus=="UnRead").ToList();
            return View(Contact);
        }
       
       
        public IActionResult ContactReply(int?id)
        {
            var GetContact = _context.ContactUs.Include(x => x.services).Where(x => x.Id == id).FirstOrDefault();
            if(GetContact==null)
            {
                return NotFound();
            }
         
            return View(GetContact);
        }
        public IActionResult ReplyUser(IFormCollection valuePairs)
        {
            int GetId =Convert.ToInt32(valuePairs["Id"]);
            string UserMessages = valuePairs["TypeMessage"];
           
            var GetEmail=_context.ContactUs.Find(GetId);
            if(GetEmail==null)
            {
                return NotFound();
            }
             GetEmail.MessagesStatus = "Read";
            _context.ContactUs.Update(GetEmail);
            _context.SaveChanges();

            var message = new ContactMessage(GetEmail.Email, "Message Form Tecno Sphere", UserMessages);

            _sender.MessageSend(message);


            return RedirectToAction("ContactList");
        }
    }
}
