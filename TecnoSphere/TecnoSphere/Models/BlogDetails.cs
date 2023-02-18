using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;

namespace TecnoSphere.Models
{
    public class BlogDetails
    {
        public int BlogID { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Slug { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? SeoDescription { get; set; }
        public string? SeoSubject { get; set; }
        public string? SeoAuthor { get; set; }
        public int? Featured { get; set; }
        public string? BlogContent { get; set; }
        public int? Comment { get; set; }
        public int? Visibility { get; set; }
        [Required]
        public string? Status { get; set; }
        public string BlogImage { get; set; }
        [NotMapped]
        [Required]
        public IFormFile imageFile { get; set; }
        public List<IFormFile> Blogimages { get; set; }
        [Required(ErrorMessage = "Date is required!")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Time is required!")]
        [DataType(DataType.Time)]
        public DateTime Time { get; set; }
        public string UserID { get; set; }
        public int? CategoryID { get; set; }

        public string GenerateSlug()
        {
            string phrase = string.Format("{0}", Title);

            string str = RemoveAccent(phrase).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        private string RemoveAccent(string text)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(text);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }
}
