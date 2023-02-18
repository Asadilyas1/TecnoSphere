using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing.Printing;
using TecnoSphere.Areas.Identity.Data;

namespace TecnoSphere.Models
{
    [Table("Blogs")]
    public class Blog
    {
        [Key]
        public int BlogID { get; set; }

        [Required]
        public string? Title { get; set; }
        public string? Slug { get; set; }
        [Required]
        public string? Description { get; set; }
        public string? SeoDescription { get; set; }
        public string? SeoSubject { get; set; }
        public string? SeoAuthor { get; set; }
        public int? Featured { get; set; }
        public string? BlogContent { get; set; }
        public int? CommentShow { get; set; }
        public int? Visibility { get; set; }
        public string? Status { get; set; }
        public string? BlogImage { get; set; }
        [NotMapped]
        [Required]
        public IFormFile? imageFile { get; set; }
        public string? Date { get; set; }
        public string? Time { get; set; }
        [Required]
        public string? UserID { get; set; }
        [Required]
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
        public virtual ApplicationUser User { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
