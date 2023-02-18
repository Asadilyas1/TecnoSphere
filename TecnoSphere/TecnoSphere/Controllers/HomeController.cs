using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TecnoSphere.Areas.Identity.Data;
using TecnoSphere.Helpers;
using TecnoSphere.Models;
using Microsoft.EntityFrameworkCore;
using TecnoSphere.Core.ViewModels;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using TecnoSphere.Models.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using TecnoSphere.Models.GoogleCaptcha;
using TecnoSphere.Models.EmailSender;

namespace TecnoSphere.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDBContext _dBContext;
        private readonly GoogleCaptchaService _googleCaptchaService;
        public readonly IMailSender _sender;
        public HomeController(ILogger<HomeController> logger, 
            ApplicationDBContext dBContext,
            GoogleCaptchaService googleCaptchaService,IMailSender sender)
        {
            _logger = logger;
            _dBContext = dBContext;
            _googleCaptchaService=googleCaptchaService;
            _sender = sender;
        }

      
        public IActionResult Index()
        {

            ViewBag.data = new SelectList(_dBContext.Services, "Id", "Title");
            var Service = _dBContext.Services.Where(x => x.Featured == 1).Take(6).ToList();
            var Portfolio = _dBContext.Portfolio.Where(x => x.Visibility == 1).Take(6).ToList();
            var ClientLogo = _dBContext.ClientLogos.ToList();
            var blog = _dBContext.Blogs.ToList();

            ViewBag.services = _dBContext.Services.ToList();

            ServicesViewModelcs servicesView = new ServicesViewModelcs()
            {
                services = Service,
                portfolios=Portfolio,
                blogs=blog,
                clientLogos=ClientLogo,
              

            };


            return View(servicesView);
        }

        

        [HttpGet]
        [Route("/services")]
        public IActionResult Services()
        {
            ViewBag.data = new SelectList(_dBContext.Services, "Id", "Title");
            ViewBag.services = _dBContext.Services.ToList();
            var services = (from s in _dBContext.Services select s);
           
            return View(services.ToList());
        }

       

        [HttpGet]
        [Route("/portfolio")]
        public IActionResult Portfolio()
        {
            ViewBag.data = new SelectList(_dBContext.Services, "Id", "Title");
            ViewBag.services = _dBContext.Services.ToList();
            var portfolio = (from p in _dBContext.Portfolio where p.Visibility==1 select p);
           
            return View(portfolio.ToList());
        }

        [HttpGet]
        [Route("/about")]
        public IActionResult about()
        {
            ViewBag.data = new SelectList(_dBContext.Services, "Id", "Title");
            ViewBag.services = _dBContext.Services.ToList();
            var Service = _dBContext.Services.Take(6).ToList();
           
            ServicesViewModelcs servicesView = new ServicesViewModelcs()
            {
                services = Service
            };
            return View(servicesView);
        }

        [HttpGet]
        [Route("/news")]
        public IActionResult news()
        {
            ViewBag.data = new SelectList(_dBContext.Services, "Id", "Title");
            ViewBag.services = _dBContext.Services.ToList();
            return View();
        }
        [HttpGet]
        [Route("/contactus")]
        public IActionResult contactus()
        {
            ViewBag.data = new SelectList(_dBContext.Services, "Id", "Title");
            ViewBag.services = _dBContext.Services.ToList();
            ViewBag.data = new SelectList(_dBContext.Services, "Id", "Title");

            return View();
        }


        [HttpGet]
        [Route("/services/{slug}")]
        public IActionResult SingleServices(string slug)
        {
            ViewBag.data = new SelectList(_dBContext.Services, "Id", "Title");
            ViewBag.services = _dBContext.Services.ToList();
            SingleServiceVM singleServiceVM = new()
            {
                services = (from s in _dBContext.Services where s.Slug == slug select s).FirstOrDefault(),
                Serviceslist = _dBContext.Services.ToList()
             

            };

            ViewBag.Description = singleServiceVM.services.SeoDescription;
            ViewBag.Subject = singleServiceVM.services.SeoSubject;
            ViewBag.Author = singleServiceVM.services.SeoAuthor;
            return View(singleServiceVM);
        }
        [HttpPost]
        public async Task<IActionResult> FormSave(IFormCollection valuePairs)
        {
            ViewBag.data = new SelectList(_dBContext.Services, "Id", "Title");
            ViewBag.services = _dBContext.Services.ToList();

            var CaptchaResult = await _googleCaptchaService.verifyToken(valuePairs["Token"]);
           
            if (!CaptchaResult)
            {
                return new JsonResult("This is Robot");
            }

            var con = new ContactUs()
            {
                Name = valuePairs["Name"],
                company = valuePairs["company"],
                Email = valuePairs["Email"],
                Message = valuePairs["Message"],
                Phone = valuePairs["Phone"],
                ServiceId = Convert.ToInt32(valuePairs["ServiceId"]),
                MessagesStatus = "UnRead",
                MessageDateTime = System.DateTime.Now

            };


            _dBContext.ContactUs.Add(con);

            int check = _dBContext.SaveChanges();

            var message = new Message(con.Email, "Email", "Email");

            _sender.MessageSend(message);
            if (check == 1)
            {
                return new JsonResult("Your Message is send to Admin");
            }
            else
            {
                return new JsonResult("Your Message is not send to Admin");
            }
        }


        [HttpGet]
        [Route("/Blogs/{slug}")]
        public IActionResult SingleBlog(string slug)
        {
            ViewBag.data = new SelectList(_dBContext.Services, "Id", "Title");
            ViewBag.services = _dBContext.Services.ToList();
            var id  =  _dBContext.Blogs.Where(x => x.Slug == slug).Select(x => x.BlogID).FirstOrDefault();

            SingleServiceVM singleServiceVM = new()
            {
                Blog = (from s in _dBContext.Blogs where s.Slug == slug select s).FirstOrDefault(),
                blogslist = _dBContext.Blogs.ToList(),
                BlogImages = _dBContext.BlogGalleries.Where(x => x.BlogID == id).ToList(),
                categories = _dBContext.Categories.ToList(),
            };
            ViewBag.Description = singleServiceVM.Blog.SeoDescription;
            ViewBag.Subject = singleServiceVM.Blog.SeoSubject;
            ViewBag.Author = singleServiceVM.Blog.SeoAuthor;
            ViewBag.image = "http://demo.tecnosphere.com.pk/Blogs/"  + singleServiceVM.Blog.BlogImage;

            ViewBag.blogtitle = singleServiceVM.Blog.Title;
            

            return View(singleServiceVM);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}