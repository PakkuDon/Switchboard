using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Switchboard.Models
{
    public class BoardDbContext : IdentityDbContext<ApplicationUser>
    {
        public BoardDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static BoardDbContext Create()
        {
            return new BoardDbContext();
        }
    }
}