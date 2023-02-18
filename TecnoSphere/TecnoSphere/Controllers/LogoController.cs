using Microsoft.AspNetCore.Mvc;
using TecnoSphere.Areas.Identity.Data;
using TecnoSphere.Models;

namespace TecnoSphere.Controllers
{
    public class LogoController : Controller
    {
        private readonly ApplicationDBContext context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public LogoController(ApplicationDBContext context, IWebHostEnvironment hostEnvironment)
        {
             this.context = context;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            var getData = context.ClientLogos.ToList();
            return View(getData);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Create(ClientLogo clientLogo)
        {
            var Time = System.DateTime.Now.ToString("yymmssfff");

            if (clientLogo.LogoImage != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;

                string filename = Path.GetFileNameWithoutExtension(clientLogo.LogoImage.FileName);
                string extension = Path.GetExtension(clientLogo.LogoImage.FileName);
                clientLogo.Logo = filename = filename + Time + extension;

                string path = Path.Combine(wwwRootPath + "/ClientLogo/", filename);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    clientLogo.LogoImage.CopyToAsync(fileStream);
                }
                context.ClientLogos.Add(clientLogo);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(clientLogo);
        }
        public IActionResult Edit(int?id)
        {
            var GetLogo = context.ClientLogos.Find(id);
            return View(GetLogo);
        }
        [HttpPost]
        public IActionResult Edit(ClientLogo clientLogo, IFormFile file, string OldImg)
        {


            if (file != null)
            {
                var Time = System.DateTime.Now.ToString("yymmssfff");

                string wwwRootPath = _hostEnvironment.WebRootPath;

                string filename = Path.GetFileNameWithoutExtension(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                clientLogo.Logo = filename = filename + Time + extension;

                string path = Path.Combine(wwwRootPath + "/ClientLogo/", filename);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyToAsync(fileStream);
                }
                context.ClientLogos.Update(clientLogo);

                context.SaveChanges();

                string filePath = Path.Combine(_hostEnvironment.WebRootPath, "ClientLogo", OldImg);
                System.IO.File.Delete(filePath);


                return RedirectToAction("Index");
            }
            else
            {
                clientLogo.Logo = OldImg;
                context.ClientLogos.Update(clientLogo);

                context.SaveChanges();

                return RedirectToAction("Index");
            }


        }
        [HttpPost]
        public IActionResult DeleteLogo(int? id)
        {

            var getData = context.ClientLogos.Find(id);
            if (getData == null)
            {
                return View();
            }
            context.ClientLogos.Remove(getData);
            int check =  context.SaveChanges();

            string filePath = Path.Combine(_hostEnvironment.WebRootPath, "ClientLogo", getData.Logo);
            System.IO.File.Delete(filePath);

            if(check==1)
            {
                return new JsonResult("Image delete successfully!");
            }
            return new JsonResult("Error! image not deleted.");

        }


    }
}
