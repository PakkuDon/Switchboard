using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Switchboard.Models;
using System.Net;
using System.Data;
using Microsoft.AspNet.SignalR;

namespace Switchboard.Controllers
{
    public class PostController : Controller
    {
        private BoardDbContext db = BoardDbContext.Create();

        // POST: Post/Add?channelID=5
        [System.Web.Mvc.Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult Add([Bind(Include = "Content,ChannelID")] Post post)
        {
            // Bind new post to active user and 
            // set timestamp if post passes validation
            try
            {
                post.User = db.Users.Where(u => u.UserName == User.Identity.Name).SingleOrDefault();
                if (ModelState.IsValid)
                {
                    post.DatePosted = DateTime.Now;
                    db.Posts.Add(post);
                    db.SaveChanges();

                    // Update clients in same channel
                    var context = GlobalHost.ConnectionManager.GetHubContext<Hubs.ChannelHub>();
                    context.Clients.Group(post.ChannelID.ToString()).displayNewPost(post.ID);

                    // Render new post form
                    ModelState.Clear();
                    return PartialView(new Post { ChannelID = post.ChannelID });
                }
            }
            // Report errors
            catch (DataException)
            {
                ModelState.AddModelError("",
                    "Error occurred while saving changes. Please try again.");
            }
            return PartialView(post);
        }

        public PartialViewResult View(int id)
        {
            // TODO: Validation
            var post = db.Posts.Find(id);
            return PartialView("~/Views/Post/View.cshtml", post);
        }

        //
        // GET: /Post/Edit/5
        [System.Web.Mvc.Authorize]
        public PartialViewResult Edit(int? id)
        {
            // TODO: Validation
            // Locate associated post
            var post = db.Posts.Find(id);
            return PartialView(post);
        }

        //
        // POST: /Post/Edit/5
        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [System.Web.Mvc.Authorize]
        public PartialViewResult EditPost(int? id)
        {
            // TODO: Validation
            // Locate associated post
            var post = db.Posts.Find(id);
            if (TryUpdateModel(post, new[] { "Content" }))
            {
                // If edit successful, update LastEdited and save changes
                try
                {
                    post.LastEdited = DateTime.Now;
                    db.SaveChanges();

                    // Update clients in same channel
                    var context = GlobalHost.ConnectionManager.GetHubContext<Hubs.ChannelHub>();
                    context.Clients.Group(post.ChannelID.ToString()).updatePost(post.ID);

                    return PartialView("~/Views/Post/View.cshtml", post);
                }
                catch (DataException)
                {
                    ModelState.AddModelError("",
                        "Unable to save changes. Try again.");
                }
            }
            return PartialView(post);
        }

        //
        // GET: /Post/Delete/5
        [System.Web.Mvc.Authorize]
        public PartialViewResult Delete(int? id)
        {
            // TODO: Validation
            // Throw error if id is null, post not found, 
            // or if user not authorised
            // Retrieve selected post
            var post = db.Posts.Find(id);

            // Render confirmation screen for selected post
            return PartialView(post);
        }

        //
        // POST: /Post/Delete/5
        [HttpPost]
        [System.Web.Mvc.Authorize]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public PartialViewResult DeletePost(int id)
        {
            var post = db.Posts.Find(id);

            // Attempt to delete selected post
            // On success, display message confirming deletion
            try
            {
                db.Posts.Remove(post);
                db.SaveChanges();

                // Update clients in same channel
                var context = GlobalHost.ConnectionManager.GetHubContext<Hubs.ChannelHub>();
                context.Clients.Group(post.ChannelID.ToString()).removePost(id);

                return PartialView("DeleteConfirmed");
            }
            // On error, redisplay dialog with error message and post data
            catch (DataException)
            {
                ModelState.AddModelError("",
                    "An error occurred while trying to delete this post. "
                    + "Please try again.");
            }
            return PartialView(post);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}