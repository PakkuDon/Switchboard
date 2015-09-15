using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Switchboard.Models;
using System.Net;
using System.Data;

namespace Switchboard.Controllers
{
    public class PostController : Controller
    {
        private BoardDbContext db = BoardDbContext.Create();

        // POST: Post/Add?channelID=5
        [HttpPost]
        public JsonResult Add([Bind(Include = "Content,ChannelID")] Post post)
        {
            try
            {
                // TODO: Use current user
                post.User = db.Users.Where(u => u.UserName == "BobbyTables").SingleOrDefault();
                if (ModelState.IsValid)
                {
                    post.DatePosted = DateTime.Now;
                    db.Posts.Add(post);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
            // TODO: Report errors
            catch (DataException e)
            {
                ModelState.AddModelError("", 
                    "Error occurred while saving changes. Please try again.");
                return Json(new { success = false, error = e.Message});
            }
            return Json(new { success = false });
        }
    }
}