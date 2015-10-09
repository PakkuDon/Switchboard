using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Switchboard.Models
{
    public class Flag
    {
        public int ID { get; set; }
        [Required]
        [MinLength(10)]
        public string Reason { get; set; }
        public DateTime ReportedOn { get; set; }
        public string Response { get; set; }
        [ScaffoldColumn(false)]
        public bool Active { get; set; } = true;

        // Foreign keys
        [Required]
        public int PostID { get; set; }
        
        // Navigation properties
        public virtual Post Post { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}