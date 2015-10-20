using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data;
using System.Data.Entity;

namespace Switchboard.Models.Permissions
{
    public static class PostPermissions
    {
        // TODO: See if initialization of db and userManager can be moved up
        public static bool CanView(string userID, int postID)
        {
            using (var db = BoardDbContext.Create())
            {
                UserManager<ApplicationUser> userManager 
                    = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

                var user = userManager.FindById(userID);
                var post = db.Posts.Find(postID);

                // If post deleted, post should only be visible to owner,
                // moderator or admin
                if (post.Deleted)
                {
                    if (user == null 
                        || !(user.UserName == post.User.UserName
                        || userManager.IsInRole(userID, "Moderator")
                        || userManager.IsInRole(userID, "Admin")))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public static bool CanEdit(string userID, int postID)
        {
            using (var db = BoardDbContext.Create())
            {
                UserManager<ApplicationUser> userManager 
                    = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

                var user = userManager.FindById(userID);
                var post = db.Posts.Find(postID);

                // Restrict access if initiated by unauthenticated user
                if (user == null)
                {
                    return false;
                }

                // Restrict access to people that don't own the post 
                // and don't have moderator rights
                if (user.UserName == post.User.UserName
                        || userManager.IsInRole(userID, "Moderator")
                        || userManager.IsInRole(userID, "Admin"))
                {
                    return true;
                }
                return false;
            }
        }

        public static bool CanDelete(string userID, int postID)
        {
            using (var db = BoardDbContext.Create())
            {
                UserManager<ApplicationUser> userManager 
                    = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

                var user = userManager.FindById(userID);
                var post = db.Posts.Find(postID);

                // Restrict access if initiated by unauthenticated user
                if (user == null)
                {
                    return false;
                }

                // If post already marked as deleted, return error
                if (post.Deleted)
                {
                    return false;
                }

                // Restrict access to people that don't own the post 
                // and don't have moderator rights
                if (user.UserName == post.User.UserName
                        || userManager.IsInRole(userID, "Moderator")
                        || userManager.IsInRole(userID, "Admin"))
                {
                    return true;
                }
                return false;
            }
        }

        public static bool CanUndelete(string userID, int postID)
        {
            using (var db = BoardDbContext.Create())
            {
                UserManager<ApplicationUser> userManager 
                    = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

                var user = userManager.FindById(userID);
                var post = db.Posts.Find(postID);

                // Restrict access if initiated by unauthenticated user
                if (user == null)
                {
                    return false;
                }

                // If post not marked as deleted, return error
                if (!post.Deleted)
                {
                    return false;
                }

                // Restrict access to people that don't own the post 
                // and don't have moderator rights
                if (user.UserName == post.User.UserName
                        || userManager.IsInRole(userID, "Moderator")
                        || userManager.IsInRole(userID, "Admin"))
                {
                    return true;
                }
                return false;
            }
        }

        public static bool CanFlag(string userID, int postID)
        {
            using (var db = BoardDbContext.Create())
            {
                UserManager<ApplicationUser> userManager
                    = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

                var user = userManager.FindById(userID);
                var post = db.Posts.Find(postID);

                // Restrict access if initiated by unauthenticated user
                if (user == null)
                {
                    return false;
                }

                // Reject flags on deleted posts
                if (post.Deleted)
                {
                    return false;
                }

                // Prevent owner, admins and moderators from flagging post
                if (user.UserName == post.User.UserName
                    || userManager.IsInRole(userID, "Admin") 
                    || userManager.IsInRole(userID, "Moderator"))
                {
                    return false;
                }

                // Reject access if user has already flagged this post
                if (db.Flags
                    .Include(f => f.User)
                    .Where(f => f.User.Id == userID)
                    .Any(f => f.PostID == postID))
                {
                    return false;
                }
                return true;
            }
        }
    }
}