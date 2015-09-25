using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Switchboard.Models;
using System.Data;
using System.Net;

namespace Switchboard.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ChannelController : Controller
    {
        private BoardDbContext db = BoardDbContext.Create();

        //
        // GET: /Admin/Channel
        public ActionResult Index(string searchTerm)
        {
            var channels = from c in db.Channels
                           select c;

            // Filter channels by search parameters
            if (!string.IsNullOrEmpty(searchTerm))
            {
                channels = channels.Where(c => c.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            return View(channels.OrderBy(c => c.Name).ToList());
        }

        //
        // GET: /Admin/Channel/Create
        public ActionResult Create()
        {
            return View();
        }

        // 
        // POST: /Admin/Channel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Channel channel)
        {
            // Validate model data
            try
            {
                if (ModelState.IsValid)
                {
                    // Add new channel to database and return to channel list
                    db.Channels.Add(channel);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("",
                    "Error occurred while saving changes. Please try again.");
            }
            return View(channel);
        }

        //
        // GET: /Admin/Channel/Edit/5
        public ActionResult Edit(int? id)
        {
            // Throw error on invalid request
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Render edit form with selected channel details
            var channel = db.Channels.Find(id);
            if (channel == null)
            {
                return HttpNotFound();
            }

            return View(channel);
        }

        //
        // POST: /Admin/Channel/Edit/5
        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Attempt to commit new changes
            var channel = db.Channels.Find(id);
            if (TryUpdateModel(channel, "",
                new string[] { "Name", "Description" }))
            {
                // Redirect to index on success
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DataException)
                {
                    // Display error
                    ModelState.AddModelError("", 
                        "Unable to save changes. Try again.");
                }
            }
            return View(channel);
        }

        //
        // GET: /Admin/Channel/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            // Throw error on invalid request
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // If redirected from a failed delete attempt, display error
            if (saveChangesError.GetValueOrDefault())
            {
                ModelState.AddModelError("",
                    "An error occurred while trying to delete this channel. "
                    + "Please try again.");
            }

            // Else, render confirmation screen with selected channel
            var channel = db.Channels.Find(id);
            if (channel == null)
            {
                return HttpNotFound();
            }
            return View(channel);
        }

        //
        // POST: /Admin/Channel/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            // Attempt to delete selected channel
            try
            {
                var channel = db.Channels.Find(id);
                db.Channels.Remove(channel);
                db.SaveChanges();
            }
            catch (DataException)
            {
                // Redisplay delete page and notify user of error
                return RedirectToAction("Delete", 
                    new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
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