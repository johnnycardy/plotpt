using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plotter.Tweet.Processing.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plotter.Tests.CommandTests
{
    [TestClass]
    public class SwitchTests_ToPreExisting : TestBase
    {

        [TestMethod]
        public void SwitchChart_ToPreExistingWithoutData_MessageReturned()
        {
            DBContext.Charts.Add(new Models.Chart() { Owner = "anna", Title = "chartA" });
            DBContext.Charts.Add(new Models.Chart() { Owner = "anna", Title = "chartB" });

            Tweet.Tweet reply = TestTweet(new Tweet.Tweet()
            {
                CreatorScreenName = "anna",
                Text = "switch charta"
            });

            Assert.IsNull(reply.Image);
            Assert.AreEqual(SwitchCommand.SwitchedToPreExisting("chartA"), reply.Text);
        }

        [TestMethod]
        public void SwitchChart_ToPreExistingWithData_MessageReturned()
        {
            DBContext.Charts.Add(new Models.Chart() { Id = 1, Owner = "anna", Title = "chartA" });
            DBContext.Points.Add(new Models.Point() { ChartId = 1, X = DateTime.Now, Y = 1 });
            DBContext.Points.Add(new Models.Point() { ChartId = 1, X = DateTime.Now.AddDays(1), Y = 2 });
            DBContext.Points.Add(new Models.Point() { ChartId = 1, X = DateTime.Now.AddDays(2), Y = 3 });

            DBContext.Charts.Add(new Models.Chart() { Owner = "anna", Title = "chartB" });

            Tweet.Tweet reply = TestTweet(new Tweet.Tweet()
            {
                CreatorScreenName = "anna",
                Text = "switch charta"
            });

            Assert.IsNotNull(reply.Image);
            Assert.AreEqual(SwitchCommand.SwitchedToPreExisting("chartA"), reply.Text);
        }

    }
}