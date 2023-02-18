using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace TecnoSphere.Models
{

    [Table("Comment")]
    public partial class Comment
    {
        public int CommentID { get; set; }
        public string CommentText { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public System.DateTime CommentDate { get; set; }
        public int ParentId { get; set; }

        public int? BlogID { get; set; }
        public virtual Blog Blog { get; set; }
    }
}
