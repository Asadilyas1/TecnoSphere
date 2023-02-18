#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecnoSphere.Models
{
    public class ClientLogo
    {
        [Key]
        public int LogoId { get; set; }
        [Required]
        [Display(Name ="Client Name")]
        [StringLength(100)]
        public string LogoTitle { get; set; }
       
        [Display(Name ="Client Logo Picture")]
        public string Logo { get; set; }
       
        [NotMapped]
        [Required]
        [Display(Name = "Client Logo Picture")]
        public IFormFile LogoImage { get; set; }

    }
}
