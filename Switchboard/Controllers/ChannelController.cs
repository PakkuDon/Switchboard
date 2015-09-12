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

        // GET: Channel
        [ChildActionOnly]
        public ActionResult Index()
        {
            return PartialView(db.Channels.OrderBy(c => c.Name).ToList());
        }

        // GET: Channel/5
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
            base.Dispose(disposing);
        }
    }
}