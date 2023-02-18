using System.Collections.Generic;

namespace TecnoSphere.Models
{
    public class BlogModel
    {
        ///<summary>
        /// Gets or sets Customers.
        ///</summary>
        public List<BlogData> blogs { get; set; }

        ///<summary>
        /// Gets or sets CurrentPageIndex.
        ///</summary>
        public int CurrentPageIndex { get; set; }

        ///<summary>
        /// Gets or sets PageCount.
        ///</summary>
        public int PageCount { get; set; }
    }
}
