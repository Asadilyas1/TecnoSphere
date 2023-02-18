
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace TecnoSphere.Models
{
    public class ServiceDetails
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Slug { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? ServiceContent { get; set; }
        [Required]
        public string? Icon { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        [Required]
        public IFormFile? imageFile { get; set; }
        public byte? Visibility { get; set; }
        public byte? Featured { get; set; }
        public string? Status { get; set; }
        public System.DateTime CreatedAt { get; set; }
        [Required]
        public string? SeoDescription { get; set; }
        public string? SeoSubject { get; set; }
        public string? SeoAuthor { get; set; }

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
