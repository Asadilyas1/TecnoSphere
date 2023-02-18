using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TecnoSphere.Models
{
    public class SingleFileModel : ReponseModel
    {
        [Required(ErrorMessage = "Please enter file name")]
        public string? FileName { get; set; }
        [Required(ErrorMessage = "Please select file")]
        public IFormFile File { get; set; }
        [Required]
        public int? AlbumID { get; set; }
    }
}
