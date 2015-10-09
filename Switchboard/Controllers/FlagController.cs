using Switchboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Data;

namespace Switchboard.Controllers
{
    [Authorize]
    public class FlagController : Controller
    {
        private BoardDbContext db = BoardDbContext.Create();

        //
        // GET: Flag
        public ActionResult Index()
        {
            // Retrieve flags created by current user
            var userID = User.Identity.GetUserId();
            var flags = db.Flags
                .Include(f => f.User)
                .Include(f => f.Post)
                .Include(f => f.Post.Channel)
                .Where(f => f.User.Id == userID);
            return View(flags.ToList());
        }

        //
        // GET: /Flag/Create
        public PartialViewResult Create(int postID)
        {
            return PartialView(new Flag { PostID = postID });
        }

        //
        // POST: /Flag/Create
        [HttpPost]
        public PartialViewResult Create([Bind(Include = "Reason,PostID")]Flag flag)
        {
            try
            { 
                if (ModelState.IsValid)
                {
                    // Set initial values for other fields
                    flag.ReportedOn = DateTime.Now;
                    flag.Active = true;
                    flag.User = db.Users.Where(u => u.UserName == User.Identity.Name).SingleOrDefault();

                    db.Flags.Add(flag);
                    db.SaveChanges();
                    return PartialView("CreateConfirmed");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", 
                    "An error occurred while trying to process this flag. Please try again.");
            }
            return PartialView(flag);
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