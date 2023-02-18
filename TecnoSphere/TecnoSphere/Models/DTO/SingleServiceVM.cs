#nullable disable
using TecnoSphere.Core.ViewModels;

namespace TecnoSphere.Models.DTO
{
    public class SingleServiceVM
    {
        public List<Services> Serviceslist { get; set; }

        public Services services { get; set; }

        public List<Blog> blogslist { get; set; }

        public Blog Blog { get; set; }

        public List<BlogGallery> BlogImages { get; set; }

        public List<Category> categories { get; set; }
 
    }
}
