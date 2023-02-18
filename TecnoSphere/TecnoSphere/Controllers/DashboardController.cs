using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Drawing2D;
using System.Drawing;
using TecnoSphere.Core;
using TecnoSphere.Models;
using static TecnoSphere.Core.Constants;
using System.Drawing.Imaging;
using Omu.Drawing;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TecnoSphere.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;
using static System.Reflection.Metadata.BlobBuilder;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TecnoSphere.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDBContext context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public DashboardController(ApplicationDBContext context, IWebHostEnvironment hostEnvironment)
        {
            this.context = context;
            _hostEnvironment = hostEnvironment;
        }
        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public List<SelectListItem> GetAlbum()
        {
            var albumList = (from album in context.Albums
                             select new SelectListItem()
                             {
                                 Text = album.Name,
                                 Value = album.Id.ToString(),
                             }).ToList();
            return albumList;
        }

        [HttpGet]
        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
        public IActionResult Gallery()
        {
            ViewBag.AlbumID = this.GetAlbum();
            return View();
        }
        [HttpGet]
        public PartialViewResult LoadGallery()
        {
            var galleries = context.Galleries.Include(x => x.Albums).ToList();
            return PartialView("_LoadGallery", galleries);
        }
        [HttpPost]
        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
        public ActionResult Gallery(SingleFileModel model)
        {
            var AlbumData = context.Albums.Where(x=>x.Id == model.AlbumID).FirstOrDefault();
            if(AlbumData != null)
            {
                Galleries galleries = new Galleries();
                var file = model.File;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/GalleryImages/" + AlbumData.Name);

                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var allowedExtensions = new[] {
                        ".Jpg", ".png", ".jpg", ".jpeg"
                        };
                var ext = Path.GetExtension(file.FileName);
                if (allowedExtensions.Contains(ext))
                {
                    string fileNameWithPath = Path.Combine(path, file.FileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    galleries.GalleryImage = file.FileName;
                    galleries.AlbumID = model.AlbumID;
                    context.Galleries.Add(galleries);
                    context.SaveChanges();
                }
            }
            return Json("");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteGallery(int id)
        {
            var gallery = await context.Galleries.FindAsync(id);
            if (gallery != null)
            {
                var AlbumData = context.Albums.Where(x => x.Id == gallery.AlbumID).FirstOrDefault();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/GalleryImages/" + AlbumData.Name);
                var filePath = path + gallery.GalleryImage;
                FileInfo file = new FileInfo(filePath);
                if (file.Exists)
                {
                    file.Delete();
                }
                context.Galleries.Remove(gallery);
                var i = context.SaveChanges();

                //Add default User to Role Admin    
                if (i > 0)
                {
                    return new JsonResult("Image delete successfully!");
                }
            }
            return new JsonResult("Error! image not deleted.");
        }

        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
        public IActionResult Albums()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetAlbums()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var customerData = (from tempcustomer in context.Albums
                                    select tempcustomer);
                if (!string.IsNullOrEmpty(sortColumn))
                {
                    if (!string.IsNullOrEmpty(sortColumnDirection))
                    {
                        customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                    }
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.Name.Contains(searchValue));
                }
                if (customerData != null)
                {
                    recordsTotal = customerData.Count();
                }
                var data = customerData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveAlbum(IFormCollection formData)
        {
            int result1;
            var albums = new Albums();
            albums.Name = formData["Name"];
            albums.Slug = albums.GenerateSlug();

            context.Albums.Add(albums);
            result1 = context.SaveChanges();

            if (result1 > 0)
            {
                return new JsonResult("Album saved successfully!");
            }

            return new JsonResult(result1);
        }

        [HttpPost]
        public IActionResult EditAlbum(int? id)
        {
            if (id != null)
            {
                var album = context.Albums.Where(x => x.Id == id).FirstOrDefault();
                return new JsonResult(album);
            }
            return new JsonResult("");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateAlbum(IFormCollection formData)
        {
            var album = context.Albums.Where(x => x.Id == int.Parse(formData["Id"].ToString())).FirstOrDefault();
            if (album != null)
            {
                album.Name = formData["Name"];
                album.Slug = album.GenerateSlug();
                context.Albums.Update(album);
                context.SaveChanges();

                return new JsonResult("Album update successfully!");
            }
            return new JsonResult("Error! Album not updated.");
        }

        [HttpPost]
        public IActionResult DeleteAlbum(int id)
        {
            var album = context.Albums.Where(x => x.Id == id).FirstOrDefault();
            if (album != null)
            {
                context.Albums.Remove(album);
                context.SaveChanges();
                return new JsonResult("Album delete successfully!");
            }
            return new JsonResult("Error! Album not deleted.");
        }

        [HttpGet]
        public IActionResult Services()
        {
            var services = context.Services.ToList();
            return View(services);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult AddService()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddService(ServiceDetails services, string ButtonType)
        {
            if (ModelState.IsValid)
            {
                Services service = new Services();

                service.Title = services.Title;
                service.Slug = services.GenerateSlug();
                service.Description = services.Description;
                service.ServiceContent = services.ServiceContent;
                if (services.imageFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;

                    string filename = Path.GetFileNameWithoutExtension(services.imageFile.FileName);
                    string extension = Path.GetExtension(services.imageFile.FileName);
                    service.Image = filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/ServiceImages/", filename);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await services.imageFile.CopyToAsync(fileStream);
                    }
                }
                service.Icon = services.Icon;
                service.Visibility = Convert.ToByte(services.Visibility);
                service.Featured = Convert.ToByte(services.Featured);
                service.Status = services.Status;
                service.SeoDescription = services.SeoDescription;
                service.SeoSubject = services.SeoDescription;
                service.SeoAuthor = "Tecno Sphere";
                service.CreatedAt = DateTime.Now;
                context.Services.Add(service);
                context.SaveChanges();
                if (ButtonType == "Save")
                {
                    return Redirect("~/Dashboard/Services");
                }
                else if (ButtonType == "SaveEdit")
                {
                    return RedirectToAction("EditService", "Dashboard", new { id = service.Id });
                }
            }
            return View(services);
        }
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditService(int? id)
        {
            var Services = await (from services in context.Services
                                  where services.Id == id
                                  select new ServiceDetails()
                                  {
                                      Id = services.Id,
                                      Title = services.Title,
                                      Description = services.Description,
                                      ServiceContent = services.ServiceContent,
                                      SeoDescription = services.SeoDescription,
                                      Featured = services.Featured,
                                      Visibility = services.Visibility,
                                      Status = services.Status,
                                      Icon = services.Icon,
                                      Image = services.Image
                                  }).FirstOrDefaultAsync();
            ViewBag.ServiceTitle = Services.Title;
            return View(Services);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditService(ServiceDetails service, string OldImage, string ButtonType)
        {
            Services services = context.Services.Where(x => x.Id == service.Id).FirstOrDefault();
            if (services != null)
            {
                services.Title = service.Title;
                if (string.IsNullOrEmpty(services.Slug))
                {
                    services.Slug = service.GenerateSlug();
                }
                services.Description = service.Description;
                services.ServiceContent = service.ServiceContent;
                services.SeoDescription = service.SeoDescription;
                services.SeoSubject = service.SeoDescription;
                services.SeoAuthor = "Tecno Sphere";
                services.Featured = Convert.ToByte(service.Featured);
                services.Visibility = Convert.ToByte(service.Visibility);
                services.Status = service.Status;
                services.Icon = service.Icon;

                //string.IsNullOrEmpty(OldImage) &&
                if ( service.imageFile != null)
                {
                   
                    string wwwRootPath = _hostEnvironment.WebRootPath;

                    string filename = Path.GetFileNameWithoutExtension(service.imageFile.FileName);
                    string extension = Path.GetExtension(service.imageFile.FileName);
                    services.Image= service.Image = filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/ServiceImages/", filename);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await service.imageFile.CopyToAsync(fileStream);
                    }
                
                }

                context.Services.Update(services);
                context.SaveChanges();
            }
            if (ButtonType == "UpdateClose")
            {
                return Redirect("~/Dashboard/Services");
            }
            else
            {
                return RedirectToAction("EditService", "Dashboard", new { id = service.Id });
            }
            ////ViewBag.ServiceTitle = services.Title;
            ////service.Image = services.Image;
            //return View();
        }

        [HttpPost]
        public IActionResult DeleteService(int id)
        {
            var services = context.Services.Where(x => x.Id == id).FirstOrDefault();
            if (services != null)
            {
                context.Services.Remove(services);
                context.SaveChanges();
                return new JsonResult("Service delete successfully!");
            }
            return new JsonResult("Error! Service not deleted.");
        }
        [HttpGet]
        public async Task<IActionResult> Pages()
        {
            var pages = context.Pages.ToList();
            return View(pages);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult AddPage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPage(MenuPages pages, string ButtonType)
        {
            if (ModelState.IsValid)
            {
                MenuPages menuPages = new MenuPages();

                if (menuPages != null)
                {
                    menuPages.Title = pages.Title;
                    menuPages.Slug = pages.GenerateSlug();
                    menuPages.MetaTitle = pages.MetaTitle;
                    menuPages.MetaDescription= pages.MetaDescription;
                    using (var datastream = new MemoryStream())
                    {
                        await pages.Image.CopyToAsync(datastream);
                        menuPages.MetaImage = datastream.ToArray();
                    }
                    menuPages.MenuText = pages.MenuText;
                    menuPages.Location = pages.Location;
                    menuPages.Visibility = pages.Visibility;
                    menuPages.ShowTitle = pages.ShowTitle;
                    context.Pages.Add(menuPages);
                    context.SaveChanges();
                    if (ButtonType == "Save")
                    {
                        return Redirect("~/Dashboard/Pages");
                    }
                    else if (ButtonType == "SaveEdit")
                    {
                        return RedirectToAction("EditPage", "Dashboard", new { id = menuPages.PageID });
                    }
                }
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditPage(int? id)
        {
            var Pages = await context.Pages.Where(x=>x.PageID == id).FirstOrDefaultAsync();
            ViewBag.PageTitle = Pages.Title;
            return View(Pages);
        }

        [HttpPost]
        public async Task<IActionResult> EditPage(MenuPages pages, string OldImage, string ButtonType)
        {
            MenuPages menuPages = context.Pages.Where(x => x.PageID == pages.PageID).FirstOrDefault();
            if (menuPages != null)
            {
                menuPages.Title = pages.Title;
                if (string.IsNullOrEmpty(pages.Slug))
                {
                    menuPages.Slug = pages.GenerateSlug();
                }
                menuPages.MetaTitle = pages.MetaTitle;
                menuPages.MetaDescription = pages.MetaDescription;
                if (string.IsNullOrEmpty(OldImage) && pages.Image != null)
                {
                    using (var datastream = new MemoryStream())
                    {
                        await pages.Image.CopyToAsync(datastream);
                        menuPages.MetaImage = datastream.ToArray();
                    }
                }
                menuPages.MenuText = pages.MenuText;
                menuPages.Location = pages.Location;
                menuPages.Visibility = pages.Visibility;
                menuPages.ShowTitle = pages.ShowTitle;
                context.Pages.Update(menuPages);
                context.SaveChanges();

                if (ButtonType == "UpdateClose")
                {
                    return Redirect("~/Dashboard/Pages");
                }
                else
                {
                    return RedirectToAction("EditPage", "Dashboard", new { id = menuPages.PageID });
                }
                return View();
            }
            return View();
        }

        [HttpPost]
        public IActionResult DeletePage(int id)
        {
            var pages = context.Pages.Where(x => x.PageID == id).FirstOrDefault();
            if (pages != null)
            {
                context.Pages.Remove(pages);
                context.SaveChanges();
                return new JsonResult("Page delete successfully!");
            }
            return new JsonResult("Error! Page not deleted.");
        }

        [HttpPost]
        public async Task<JsonResult> ChangeStatus(int PageID, byte Visibility)
        {
            var pages = await context.Pages.Where(x=>x.PageID == PageID).FirstOrDefaultAsync();
            if(pages != null)
            {
                pages.Visibility = Visibility;
                context.Pages.Update(pages);
                context.SaveChanges();
                return Json("Visibility changed successfully");
            }
            return Json("");
        }
        [HttpGet]
        public async Task<IActionResult> Portfolio()
        {
            var portfolio = context.Portfolio.ToList();
            return View(portfolio);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult Skills()
        {
            return View();
        }

        [HttpGet]
        public JsonResult LoadSkills()
        {
            return Json("");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult AddPortfolio()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPortfolio(Portfolio portfolio, string ButtonType)
        {
            if (ModelState.IsValid)
            {
                Portfolio portfolio1 = new Portfolio();
                portfolio1.Title = portfolio.Title;
                portfolio1.Slug = portfolio.GenerateSlug();
                portfolio1.Link = portfolio.Link;
                portfolio1.Description = portfolio.Description;
                portfolio1.MetaTitle = portfolio.MetaTitle;
                portfolio1.MetaDescription = portfolio.MetaDescription;
                if (portfolio.PortfolioImage != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;

                    string filename = Path.GetFileNameWithoutExtension(portfolio.PortfolioImage.FileName);
                    string extension = Path.GetExtension(portfolio.PortfolioImage.FileName);
                    portfolio1.Image =  portfolio.Image = filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                   
                    string path = Path.Combine(wwwRootPath + "/Portfolio/", filename);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await portfolio.PortfolioImage.CopyToAsync(fileStream);
                    }
                }
                portfolio1.Visibility = portfolio.Visibility;
                context.Portfolio.Add(portfolio1);
                context.SaveChanges();
                if (ButtonType == "Save")
                {
                    return Redirect("~/Dashboard/Portfolio");
                }
                else if (ButtonType == "SaveEdit")
                {
                    return RedirectToAction("EditPortfolio", "Dashboard", new { id = portfolio1.PortfolioID });
                }
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditPortfolio(int? id)
        {
            var Portfolio = await context.Portfolio.Where(x => x.PortfolioID == id).FirstOrDefaultAsync();
            ViewBag.PortfolioTitle = Portfolio.Title;
            return View(Portfolio);
        }
        [HttpPost]
        public async Task<IActionResult> EditPortfolio(Portfolio pages, string OldImage, string ButtonType)
        {
            Portfolio menuPages = context.Portfolio.Where(x => x.PortfolioID == pages.PortfolioID).FirstOrDefault();
            if (menuPages != null)
            {
                menuPages.Title = pages.Title;
                if (string.IsNullOrEmpty(pages.Slug))
                {
                    menuPages.Slug = pages.GenerateSlug();
                }
                menuPages.Link = pages.Link;
                menuPages.MetaTitle = pages.MetaTitle;
                menuPages.MetaDescription = pages.MetaDescription;
                menuPages.Description = pages.Description;
                //string.IsNullOrEmpty(OldImage) &&


                if (pages.PortfolioImage != null)
                {
                    
                    string wwwRootPath = _hostEnvironment.WebRootPath;

                    string filename = Path.GetFileNameWithoutExtension(pages.PortfolioImage.FileName);
                    string extension = Path.GetExtension(pages.PortfolioImage.FileName);
                    menuPages.Image=   pages.Image = pages.Image = filename = filename + DateTime.Now.ToString("yymmssfff") + extension;

                    string path = Path.Combine(wwwRootPath + "/Portfolio/", filename);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await pages.PortfolioImage.CopyToAsync(fileStream);
                    }
                
                }
                menuPages.Visibility = pages.Visibility;
                context.Portfolio.Update(menuPages);
                context.SaveChanges();

                if (ButtonType == "UpdateClose")
                {
                    return Redirect("~/Dashboard/Portfolio");
                }
                else
                {
                    return RedirectToAction("EditPortfolio", "Dashboard", new { id = menuPages.PortfolioID });
                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult DeletePortfolio(int id)
        {
            var portfolio = context.Portfolio.Where(x => x.PortfolioID == id).FirstOrDefault();
            if (portfolio != null)
            {
                context.Portfolio.Remove(portfolio);
                context.SaveChanges();
                return new JsonResult("Page delete successfully!");
            }
            return new JsonResult("Error! Page not deleted.");
        }

        [HttpPost]
        public async Task<JsonResult> PortfolioStatus(int PortfolioID, byte Visibility)
        {
            var portfolio = await context.Portfolio.Where(x => x.PortfolioID == PortfolioID).FirstOrDefaultAsync();
            if (portfolio != null)
            {
                portfolio.Visibility = Visibility;
                context.Portfolio.Update(portfolio);
                context.SaveChanges();
                return Json("Visibility changed successfully");
            }
            return Json("");
        }

     
    }
}
