#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecnoSphere.Models
{
    public class ContactUs
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(100)]
        [Required]
        public string Name { get; set; }
        [StringLength(50)]
        [Required]
        public string Phone { get; set; }
        [StringLength(200)]
        [Required]
        public string company { get; set; }

        [Required]
        public string Message { get; set; }

        public int ServiceId { get; set; }

        [ForeignKey("ServiceId")]

        public Services services { get; set; }

        //Check token is valid or not for Google Capthca
        [NotMapped]

        public string Token { get; set; }

        public string MessagesStatus { get; set; }

        public DateTime MessageDateTime { get; set; }
    }
}
