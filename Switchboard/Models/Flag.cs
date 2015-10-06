using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Switchboard.Models
{
    public class Flag
    {
        public int ID { get; set; }
        public string Reason { get; set; }
        public DateTime ReportedOn { get; set; }
        public bool Active { get; set; } = true;

        // Foreign keys
        public int PostID { get; set; }
        
        // Navigation properties
        public virtual Post Post { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}