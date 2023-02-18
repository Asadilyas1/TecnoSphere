using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace TecnoSphere.Models
{
    [Table("Pages")]
    public class MenuPages
    {
        [Key]
        public int PageID { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Slug { get; set; }
        [Required]
        public string? MetaTitle { get; set; }
        [Required]
        public string? MetaDescription { get; set; }
        [NotMapped]
        [Required]
        public IFormFile Image { get; set; }
        public byte[]? MetaImage { get; set; }
        public string? MenuText { get; set; }
        public string? Location { get; set; }
        public byte? Visibility { get; set; }
        public byte? ShowTitle { get; set; }
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
