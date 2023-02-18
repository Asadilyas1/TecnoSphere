using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace TecnoSphere.Models
{
    public class Portfolio
    {
        [Key]
        public int PortfolioID { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Slug { get; set; }
        [Required]
        public string? Link { get; set; }
        [Required]
        [NotMapped]
        public IFormFile? PortfolioImage { get; set; }
        public string? Image { get; set; }
        [Required]
        public string? Description { get; set; }
        public byte? Visibility { get; set; }
        [Required]
        public string? MetaTitle { get; set; }
        [Required]
        public string? MetaDescription { get; set; }
        public string? MetaImage { get; set; }
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
