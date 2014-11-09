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
            bool isAwake = TweetIO.IsAwake;
            if (isAwake)
            {
                return Content("Already awake");
            }
            else
            {
                TweetIO.WakeUp();
                return Content("Woken up");
            }
        }
    }
}