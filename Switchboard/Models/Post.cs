using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Switchboard.Models
{
    public class Post
    {
        public int ID { get; set; }
        [MinLength(30)]
        public string Content { get; set; }
        public DateTime DatePosted { get; set; }

        // Navigation properties
        public virtual Channel Channel { get; set; }
        [Required]
        public virtual ApplicationUser User { get; set; }
    }
}