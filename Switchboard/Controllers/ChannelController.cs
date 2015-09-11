using Switchboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}