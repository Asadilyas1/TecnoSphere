using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecnoSphere.Models
{
    [Table("BlogGallery")]
    public class BlogGallery
    {
        [Key]
        public int GalleryID { get; set; }  
        public int BlogID { get; set; }
        public string Images { get; set; }

        public virtual Blog Blog { get; set; }  
    }
}
