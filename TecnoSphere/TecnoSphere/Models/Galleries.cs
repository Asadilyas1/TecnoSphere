using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecnoSphere.Models
{
    [Table("Galleries")]
    public class Galleries
    {
        [Key]
        public int GalleryID { get; set; }
        public string? GalleryImage { get; set; }
        public int? AlbumID { get; set; }
        [ForeignKey(nameof(AlbumID))]
        public virtual Albums Albums { get; set; }
    }
}
