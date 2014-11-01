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
    public class AddPoint : TestBase
    {

        [TestMethod]
        public void SinglePoint_First_Chart_ChartCreated_ButNotReturned()
        {
            var result = TestTweet(new Tweet.Tweet()
            {
                CreatorScreenName = "anna",
                Text = "10"
            });

            Assert.AreEqual("@anna " + AddPointCommand.VeryFirstPoint_ChartCreated , result.GetMessageForSending());
            Assert.IsNull(result.Image);
            Assert.AreEqual(1, DBContext.Charts.Count());
            Assert.AreEqual(1, DBContext.Points.Count());
        }

        [TestMethod]
        public void SecondPoint_First_Chart_Returned_NoMessage()
        {
            DBContext.Charts.Add(new Models.Chart() { Id = 4, Owner = "anna" });
            DBContext.Points.Add(new Models.Point() { ChartId = 4, X = DateTime.Now, Y = 5 });

            var result = TestTweet(new Tweet.Tweet()
            {
                CreatorScreenName = "anna",
                Text = "10"
            });

            Assert.AreEqual("@anna", result.GetMessageForSending());
            Assert.IsNotNull(result.Image);
            Assert.AreEqual(1, DBContext.Charts.Count());
            Assert.AreEqual(2, DBContext.Points.Count());
            Assert.AreEqual(2, DBContext.Points.Count(p => p.ChartId == 4));
        }

        [TestMethod]
        public void FirstPointAdded_MultipleCharts_LastChartUsed()
        {
            DBContext.Charts.Add(new Models.Chart() { Id = 1, Owner = "anna" });
            DBContext.Charts.Add(new Models.Chart() { Id = 2, Owner = "anna" });
            DBContext.Points.Add(new Models.Point() { ChartId = 1, X = DateTime.Now, Y = 5 });

            var result = TestTweet(new Tweet.Tweet()
            {
                CreatorScreenName = "anna",
                Text = "10"
            });

            Assert.IsNull(result.Image);
            Assert.AreEqual(2, DBContext.Charts.Count());
            Assert.AreEqual(2, DBContext.Points.Count());
            Assert.AreEqual(1, DBContext.Points.Count(p => p.ChartId == 2));
        }



        [TestMethod]
        public void FirstPointAdded_ExistingChart_MessageReturned()
        {
            DBContext.Charts.Add(new Models.Chart() { Id = 1, Owner = "anna" });

            var result = TestTweet(new Tweet.Tweet()
            {
                CreatorScreenName = "anna",
                Text = "10"
            });

            Assert.AreEqual("@anna " + AddPointCommand.FirstPoint_ExistingChart, result.GetMessageForSending());
            Assert.IsNull(result.Image);
            Assert.AreEqual(1, DBContext.Charts.Count());
            Assert.AreEqual(1, DBContext.Points.Count());
        }
    }
}
