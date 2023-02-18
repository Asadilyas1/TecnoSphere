using System.Collections.Generic;

namespace TecnoSphere.Models
{
    public class DetailData
    {
        public virtual Blog Blog { get; set; }

        public virtual IEnumerable<BlogGallery> BlogGallery { get; set; }
    }
}
