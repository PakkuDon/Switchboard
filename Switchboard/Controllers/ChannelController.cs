using Switchboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Switchboard.Controllers
{
    public class ChannelController : Controller
    {
        private BoardDbContext db = BoardDbContext.Create();

        //
        // GET: Channel/
        public ActionResult Index(string searchTerm)
        {
            var channels = from c in db.Channels
                           select c;

            // If search term provided, filter results
            if (!string.IsNullOrEmpty(searchTerm))
            {
                channels = channels.Where(
                    c => c.Name.ToLower().Contains(searchTerm.ToLower())
                    || c.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            return PartialView(channels.OrderBy(c => c.Name).ToList());
        }

        //
        // GET: Channel/View/5
        public ActionResult View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Retrieve selected channel and eager load posts
            var channel = db.Channels.Find(id);
            if (channel == null)
            {
                return HttpNotFound();
            }
            db.Entry(channel).Collection(c => c.Posts);

            return View(channel);
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