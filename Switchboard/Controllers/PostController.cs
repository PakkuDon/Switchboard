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
        [Authorize]
        [HttpPost]
        public JsonResult Add([Bind(Include = "Content,ChannelID")] Post post)
        {
            // Bind new post to active user and set timestamp 
            // if post passes validation
            try
            {
                post.User = db.Users.Where(u => u.UserName == User.Identity.Name).SingleOrDefault();
                if (ModelState.IsValid)
                {
                    post.DatePosted = DateTime.Now;
                    db.Posts.Add(post);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
            // Report errors
            catch (DataException e)
            {
                ModelState.AddModelError("",
                    "Error occurred while saving changes. Please try again.");
                return Json(new { success = false, errors = new List<string> { e.Message } });
            }
            var errors = ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .SelectMany(e => e.Value.Errors
                .Select(x => x.ErrorMessage));
            return Json(new { success = false, errors = errors });
        }
    }
}