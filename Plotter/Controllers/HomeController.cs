using Plotter.Tweet;
using Plotter.Tweet.Processing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Plotter.Controllers
{
    public class HomeController : Controller
    {
        private static TweetIO _tweetIO;

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
