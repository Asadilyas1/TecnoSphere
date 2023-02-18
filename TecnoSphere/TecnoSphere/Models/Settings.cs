using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecnoSphere.Models
{
    [Table("Settings")]
    public class Settings
    {
        [Key]
        public int AppID { get; set; }
        public string AppName { get; set; }
        public byte[] Logo { get; set; }
        public string HeaderText { get; set; }
        public string FooterText { get; set; }
        public string AboutText { get; set; }
        public string ContactText { get; set; }
        public string Map { get; set; }
        public string ContactNumber { get; set; }
        public string Location { get; set; }
        public string Day { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
