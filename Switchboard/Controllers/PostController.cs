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

                    // Update all active clients
                    var context = GlobalHost.ConnectionManager.GetHubContext<Hubs.ChannelHub>();
                    context.Clients.All.displayNewPost(post.ID);

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

        public PartialViewResult View(int id)
        {
            // TODO: Validation
            var post = db.Posts.Find(id);
            return PartialView("~/Views/Post/View.cshtml", post);
        }

        //
        // GET: /Post/Edit/5
        [System.Web.Mvc.Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }

            if (post.User.UserName != User.Identity.Name)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            return View(post);
        }

        //
        // POST: /Post/Edit/5
        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var post = db.Posts.Find(id);
            if (post.User.UserName != User.Identity.Name)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            if (TryUpdateModel(post, new[] { "Content" }))
            {
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("View", "Channel", new { id = post.ChannelID });
                }
                catch (DataException)
                {
                    ModelState.AddModelError("",
                        "Unable to save changes. Try again.");
                }
            }
            return View(post);
        }
    }
}