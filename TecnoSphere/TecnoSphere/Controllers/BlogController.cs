using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TecnoSphere.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using TecnoSphere.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Dynamic.Core;
using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.Security.Cryptography;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata;

namespace TecnoSphere.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BlogController(ApplicationDBContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var Blog = await (from blog in _context.Blogs
                        join users in _context.Users on blog.UserID equals users.Id
                        join categories in _context.Categories on blog.CategoryID equals categories.Id
                        select new BlogData()
                        {
                            BlogID = blog.BlogID,
                            Title = blog.Title,
                            Image = blog.BlogImage,
                            CategoryName = categories.Name,
                            UserName = users.FirstName + " " + users.LastName,
                            Date = blog.Date,
                            Status = blog.Status
                        }).ToListAsync();
            return View(Blog);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult AddBlog()
        {
            ViewBag.BlogID = "";
            ViewBag.CategoryID = _context.Categories.ToList();
            var users = GetUser();
            ViewBag.UserID = users;
            return View();
        }

        [HttpPost]
        //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddBlog(BlogDetails blog, string ButtonType)
        {
            //if (ModelState.IsValid)
            //{
                var userData = _context.Users.Where(x=>x.Id == blog.UserID).FirstOrDefault();
                if (!string.IsNullOrEmpty(blog.CategoryID.ToString()))
                {
                    Blog blog1 = new Blog();

                    blog1.Title = blog.Title;
                    blog1.Slug = blog.GenerateSlug();
                    blog1.Description = blog.Description;
                    blog1.SeoDescription = blog.SeoDescription;
                    blog1.SeoSubject = blog.SeoDescription;
                    blog1.SeoAuthor = userData.FirstName + " " + userData.LastName;
                    blog1.Featured = Convert.ToInt32(blog.Featured);
                    blog1.BlogContent = blog.BlogContent;
                    blog1.CommentShow = Convert.ToInt32(blog.Comment);
                    blog1.Visibility = blog.Visibility;
                    blog1.Status = blog.Status;
                   
                        if (blog.imageFile != null)
                        {
                            string wwwRootPath = _hostEnvironment.WebRootPath;

                            string filename = Path.GetFileNameWithoutExtension(blog.imageFile.FileName);
                            string extension = Path.GetExtension(blog.imageFile.FileName);
                            blog1.BlogImage =   blog.BlogImage = filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                            string path = Path.Combine(wwwRootPath + "/Blogs/", filename);
                            using (var fileStream = new FileStream(path, FileMode.Create))
                            {
                                await blog.imageFile.CopyToAsync(fileStream);
                            }
                        }
                    
                    blog1.Date = blog.Date.ToShortDateString();
                    blog1.Time = blog.Time.ToShortTimeString();
                    blog1.UserID = blog.UserID;
                    blog1.CategoryID = (int)blog.CategoryID;
                    _context.Blogs.Add(blog1);
                    _context.SaveChanges();

                    if (blog.Blogimages.Count > 0)
                    {

                        foreach (var item in blog.Blogimages)
                        {
                           var dateTime = System.DateTime.Now.ToString("yymmssfff");
                            BlogGallery blogGallery = new BlogGallery();

                        string wwwRootPath = _hostEnvironment.WebRootPath;

                        string filename = Path.GetFileNameWithoutExtension(item.FileName);
                        string extension = Path.GetExtension(item.FileName);
                        blogGallery.Images = blogGallery.Images = filename = filename + dateTime + extension;
                        string path = Path.Combine(wwwRootPath + "/Uploads/BlogGallery/", filename);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await item.CopyToAsync(fileStream);
                        }
                      

                            blogGallery.BlogID = blog1.BlogID;

                            _context.BlogGalleries.Add(blogGallery);
                            _context.SaveChanges();
                        }
                    }
                    if (ButtonType == "Save")
                    {
                        return Redirect("~/Blog/Index");
                    }
                    else if (ButtonType == "SaveEdit")
                    {
                        return RedirectToAction("EditBlog", "Blog", new { id = blog.BlogID });
                    }
                }
                else
                {
                    ViewBag.Error = "Category Name is required!";
                }
            //}
            ViewBag.CategoryID = _context.Categories.ToList();
            var users = GetUser();
            ViewBag.UserID = users;
            return View(blog);
        }

        public List<SelectListItem> GetUser()
        {
            var productsList = (from product in _context.Users
                                join userRoles in _context.UserRoles on product.Id equals userRoles.UserId
                                join roles in _context.Roles on userRoles.RoleId equals roles.Id
                                //where roles.Name != "Administrator"
                                select new SelectListItem()
                                {
                                    Text = product.FirstName + " " + product.LastName,
                                    Value = product.Id,
                                }).ToList();
            return productsList;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult Categories()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditBlog(int? id)
        {
            var Blog = (from blog in _context.Blogs
                              where blog.BlogID == id
                              select new BlogDetails()
                              {
                                  BlogID = blog.BlogID,
                                  Title = blog.Title,
                                  Description = blog.Description,
                                  SeoDescription = blog.SeoDescription,
                                  Featured = blog.Featured,
                                  Comment = blog.CommentShow,
                                  Visibility = blog.Visibility,
                                  BlogContent = blog.BlogContent,
                                  Date = Convert.ToDateTime(blog.Date),
                                  Time = Convert.ToDateTime(blog.Time),
                                  Status = blog.Status,
                                  UserID = blog.UserID,
                                  CategoryID = blog.CategoryID,
                                  BlogImage = blog.BlogImage
                              }).FirstOrDefault();
            ViewBag.CategoryID = await _context.Categories.ToListAsync();
            var users = GetUser();
            ViewBag.UserID = users;
            ViewBag.BlogTitle = Blog.Title;
            return View(Blog);
        }

        [HttpGet]
        public PartialViewResult LoadCategories()
        {
            var categories = _context.Categories.ToList();
            return PartialView("~/Views/Blog/_LoadCategories.cshtml", categories);
        }

        [HttpPost]
        public IActionResult GetCategories()
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
                var customerData = (from tempcustomer in _context.Categories
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
                if(customerData != null)
                {
                    recordsTotal = customerData.Count();
                }
                var data = customerData.Skip(skip).Take(pageSize).OrderBy(x => x.Ord).ToList();
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
        public IActionResult SaveCategory(IFormCollection formData)
        {
            int result1;
            var category = new Category();
            category.Name = formData["Name"];
            category.Slug = category.GenerateSlug();
            category.Description = formData["Description"];
            category.Status = formData["Status"];
            category.Icon = formData["Icon"];
            category.Ord = byte.Parse(formData["Order"].ToString());
            category.IsFeatured = byte.Parse(formData["Featured"].ToString());
            category.SeoDescription = formData["SeoDescription"];
            category.SeoSubject = formData["SeoDescription"];
            category.SeoAuthor = "Tecno Sphere";
            category.CreatedAt = DateTime.Now;

            _context.Categories.Add(category);
            result1 = _context.SaveChanges();

            if (result1 > 0)
            {
                return new JsonResult("Category saved successfully!");
            }

            return new JsonResult(result1);
        }

        [HttpPost]
        public IActionResult EditCategory(int? id)
        {
            if (id != null)
            {
                var category = _context.Categories.Where(x=>x.Id == id).FirstOrDefault();
                return new JsonResult(category);
            }
            return new JsonResult("");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCategory(IFormCollection formData)
        {
            var category = _context.Categories.Where(x => x.Id == int.Parse(formData["Id"].ToString())).FirstOrDefault();
            if (category != null)
            {
                category.Name = formData["Name"];
                category.Description = formData["Description"];
                category.Status = formData["Status"];
                category.Icon = formData["Icon"];
                category.Ord = byte.Parse(formData["Order"].ToString());
                category.IsFeatured = byte.Parse(formData["Featured"].ToString());
                category.SeoDescription = formData["SeoDescription"];
                category.SeoSubject = formData["SeoDescription"];
                category.SeoAuthor = "Tecno Sphere";
                category.UpdatedAt = DateTime.Now;
                _context.Categories.Update(category);
                _context.SaveChanges();

                return new JsonResult("Category update successfully!");
            }
            return new JsonResult("Error! Category not updated.");
        }

        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.Where(x => x.Id == id).FirstOrDefault();
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
                return new JsonResult("Category delete successfully!");
            }
            return new JsonResult("Error! Category not deleted.");
        }

        [HttpGet]
        public async Task<JsonResult> ShowGallery(int? id)
        {
            var Gallery = await _context.BlogGalleries.Where(x => x.BlogID == id).ToListAsync();
            return Json(Gallery);
        }

        [HttpPost]
        //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditBlog(BlogDetails blog, string OldImage, List<string> GalleryImages, string ButtonType)
        {
            Blog blog1 = _context.Blogs.Where(x => x.BlogID == blog.BlogID).FirstOrDefault();
            var userData = _context.Users.Where(x => x.Id == blog1.UserID).FirstOrDefault();
            if (blog1 != null)
            {
                blog1.Title = blog.Title;
                if (string.IsNullOrEmpty(blog1.Slug))
                {
                    blog1.Slug = blog.GenerateSlug();
                }
                blog1.Description = blog.Description;
                blog1.SeoDescription = blog.SeoDescription;
                blog1.SeoSubject = blog.SeoDescription;
                blog1.SeoAuthor = userData.FirstName + " " + userData.LastName;
                blog1.Featured = Convert.ToInt32(blog.Featured);
                blog1.BlogContent = blog.BlogContent;
                blog1.CommentShow = Convert.ToInt32(blog.Comment);
                blog1.Visibility = blog.Visibility;
                blog1.Status = blog.Status;
                blog1.Date = blog.Date.ToShortDateString();
                blog1.Time = blog.Time.ToShortTimeString();
                blog1.UserID = blog.UserID;
                blog1.CategoryID = (int)blog.CategoryID;
                //string.IsNullOrEmpty(OldImage) &&
                if (blog.imageFile != null)
                {

                    string wwwRootPath = _hostEnvironment.WebRootPath;

                    string filename = Path.GetFileNameWithoutExtension(blog.imageFile.FileName);
                    string extension = Path.GetExtension(blog.imageFile.FileName);
                    blog1.BlogImage = blog.BlogImage = filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/Blogs/", filename);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await blog.imageFile.CopyToAsync(fileStream);
                    }

                }

                _context.Blogs.Update(blog1);
                _context.SaveChanges();

                /*GalleryImages.Count() == 0 && */

                if (blog.Blogimages!=null)
                {
                    foreach (var item in blog.Blogimages)
                    {
                        BlogGallery blogGallery = new BlogGallery();
                        string file = Path.GetFileNameWithoutExtension(item.FileName);
                        string extens = Path.GetExtension(item.FileName);
                        var FileN = file + DateTime.Now.ToString("yymmssfff") + extens;
                        blogGallery.Images = FileN;
                        string pathI = Path.Combine(_hostEnvironment.WebRootPath + "/Uploads/BlogGallery/", FileN);
                        using (var fileStream = new FileStream(pathI, FileMode.Create))
                        {
                            await item.CopyToAsync(fileStream);
                        }

                        blogGallery.BlogID = blog1.BlogID;

                        _context.BlogGalleries.Add(blogGallery);
                        _context.SaveChanges();
                    }
                }
            }
            if (ButtonType == "UpdateClose")
            {
                return Redirect("~/Blog/Index");
            }
            ViewBag.CategoryID = _context.Categories.ToList();
            var users = GetUser();
            ViewBag.UserID = users;
            ViewBag.BlogTitle = blog1.Title;
            blog.BlogImage = blog1.BlogImage;
            return View(blog);
        }

        public async Task<JsonResult> DeleteImages(int? id)
        {
            var gallery = await _context.BlogGalleries.Where(x => x.GalleryID == id).FirstOrDefaultAsync();
            if (gallery != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/BlogGallery/");
                var filePath = path + gallery.Images;
                FileInfo file = new FileInfo(filePath);
                if (file.Exists)
                {
                    file.Delete();
                }
                _context.BlogGalleries.Remove(gallery);
                _context.SaveChanges();
                return Json(true);
            }
            return Json(false);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteBlog(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null)
            {
                _context.Blogs.Remove(blog);
                int i = _context.SaveChanges();

                var gallery = _context.BlogGalleries.Where(x => x.BlogID == id).ToList();
                foreach(var item in gallery)
                {
                    string pathI = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/BlogGallery/");
                    var filePathI = pathI + item.Images;
                    FileInfo fileI = new FileInfo(filePathI);
                    if (fileI.Exists)
                    {
                        fileI.Delete();
                    }
                    _context.BlogGalleries.Remove(item);
                    _context.SaveChanges();
                }

                if (i > 0)
                {
                    return new JsonResult("Image delete successfully!");
                }
            }
            return new JsonResult("Error! image not deleted.");
        }
        [HttpPost]
        public IActionResult UploadImage(IFormFile upload)
        {
            if (upload != null && upload.Length > 0)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + upload.FileName;
                var path = Path.Combine(Directory.GetCurrentDirectory(), wwwRootPath + "/BlogEditorImg/", fileName);
                var stream = new FileStream(path, FileMode.Create);
                upload.CopyToAsync(stream);
                return new JsonResult(new { path = "/BlogEditorImg/" + fileName });
            }
            return View("AddBlog");
        }
        [HttpGet]

        public IActionResult UploadExplorer()
        {
            var dir = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), _hostEnvironment.WebRootPath + "/BlogEditorImg/"));
            ViewBag.fileInfo = dir.GetFiles();
            return View("FileExplorer");
        }

        public IActionResult FileExplorer()
        {
            return View();
        }
    }
}
