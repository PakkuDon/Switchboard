using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Switchboard.Models;
using System.Data.Entity;
using System.Data;

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

        //
        // POST: Moderator/Flag/Resolve
        public PartialViewResult Resolve(int? id)
        {
            var flag = db.Flags.Find(id);

            if (TryUpdateModel(flag, new[] { "Response" }))
            {
                try
                { 
                    flag.Active = false;
                    db.SaveChanges();
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", 
                        "An error occured while trying to save response. Please try again.");
                }
            }
            return PartialView("View", flag);
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