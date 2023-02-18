using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace TecnoSphere.Models
{
    [Table("BlogCategory")]
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }    
        public string? Slug { get; set; }    
        public string? Description { get; set; }
        [Required]
        public string? Status { get; set; }    
        public string? Icon { get; set; }    
        public byte? Ord { get; set; }    
        public byte? IsFeatured { get; set; }
        public string? SeoDescription { get; set; }
        public string? SeoSubject { get; set; }
        public string? SeoAuthor { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }    

        public virtual ICollection<Blog>? Blogs { get; set; }

        public string GenerateSlug()
        {
            string phrase = string.Format("{0}", Name);

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
