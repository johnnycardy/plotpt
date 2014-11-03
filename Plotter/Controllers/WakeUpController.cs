using Plotter.Tweet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Plotter.Controllers
{
    public class WakeUpController : Controller
    {
        // GET: WakeUp
        public ActionResult Index()
        {
            //Wait it up by making sure instance is initialised
            return View(TweetIO.Instance);
        }
    }
}