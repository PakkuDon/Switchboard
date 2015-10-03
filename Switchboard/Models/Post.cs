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
        [Required]
        [MinLength(10)]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        public DateTime DatePosted { get; set; }
        public DateTime? LastEdited { get; set; }
        public bool Deleted { get; set; }

        // Foreign keys
        public int ChannelID { get; set; }

        // Navigation properties
        public virtual Channel Channel { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}