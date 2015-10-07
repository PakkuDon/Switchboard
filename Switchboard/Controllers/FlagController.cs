using Switchboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace Switchboard.Controllers
{
    [Authorize]
    public class FlagController : Controller
    {
        private BoardDbContext db = BoardDbContext.Create();

        // GET: Flag
        public ActionResult Index()
        {
            // Retrieve flags created by current user
            var flags = db.Flags
                .Include(f => f.User)
                .Include(f => f.Post)
                .Include(f => f.Post.Channel)
                .Where(f => f.User.Id == User.Identity.GetUserId());
            return View(flags.ToList());
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