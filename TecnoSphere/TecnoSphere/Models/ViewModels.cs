using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecnoSphere.Models
{
    public class ViewModels
    {
        public int ComID { get; set; }
        public string CommentMsg { get; set; }
        public DateTime CommentedDate { get; set; }
        public int? BlogID { get; set; }
        public int? UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
