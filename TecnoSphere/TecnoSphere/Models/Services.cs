
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecnoSphere.Models
{
    public class Services
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
        public string? Image { get; set; }
        [NotMapped]
        [Required]
        public IFormFile? imageFile { get; set; }
        [Required]
        public string? Icon { get; set; }
        public byte? Visibility { get; set; }
        public byte? Featured { get; set; }
        public string? Status { get; set; }
        public System.DateTime CreatedAt { get; set; }
        [Required]
        public string? SeoDescription { get; set; }
        public string? SeoSubject { get; set; }
        public string? SeoAuthor { get; set; }
    }
}
