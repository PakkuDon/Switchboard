using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Switchboard.Models.Permissions
{
    public static class PostPermissions
    {
        // TODO: See if initialization of db and userManager can be moved up
        public static bool CanView(string userID, int postID)
        {
            BoardDbContext db = BoardDbContext.Create();
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            var user = userManager.FindById(userID);
            var post = db.Posts.Find(postID);
            bool result = true;

            if (post.Deleted)
            {
                if (user == null || !(user.UserName != post.User.UserName 
                    || userManager.IsInRole(userID, "Moderator")
                    || userManager.IsInRole(userID, "Admin")))
                {
                    result = false;
                }
            }

            db.Dispose();
            return result;
        }

        public static bool CanEdit(string userID, int postID)
        {
            return false;
        }

        public static bool CanDelete(string userID, int postID)
        {
            return false;
        }

        public static bool CanUndelete(string userID, int postID)
        {
            return false;
        }

        public static bool CanFlag(string userID, int postID)
        {
            return false;
        }
    }
}