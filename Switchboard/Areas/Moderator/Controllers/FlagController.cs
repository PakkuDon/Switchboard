using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Switchboard.Models;
using System.Data.Entity;

namespace Switchboard.Areas.Moderator.Controllers
{
    [Authorize(Roles = "Admin,Moderator")]
    public class FlagController : Controller
    {
        private BoardDbContext db = BoardDbContext.Create();

        // GET: Moderator/Flag
        public ActionResult Index()
        {
            var flags = db.Flags
                .Include(f => f.Post)
                .Include(f => f.User)
                .OrderByDescending(f => f.ReportedOn);
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