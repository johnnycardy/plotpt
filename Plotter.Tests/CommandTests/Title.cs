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
    public class Title : TestBase
    {

        [TestMethod]
        public void SetTitle_Invalid_MessageReturned()
        {
            Tweet.Tweet reply = TestTweet(new Tweet.Tweet()
            {
                CreatorScreenName = "anna",
                Text = "title"
            });

            Assert.IsNull(reply.Image);
            Assert.AreEqual(TitleCommand.InvalidCommandMessage, reply.Text);
        }


        [TestMethod]
        public void SetTitle_NoChart_MessageReturned()
        {
            Tweet.Tweet reply = TestTweet(new Tweet.Tweet()
            {
                CreatorScreenName = "anna",
                Text = "title bob"
            });

            Assert.IsNull(reply.Image);
            Assert.AreEqual("bob", DBContext.Charts.First().Title);
            Assert.AreEqual(TitleCommand.NewChartTitleSet("bob"), reply.Text);
        }

        [TestMethod]
        public void SetTitle_ExistingUnnamedChart_MessageReturned()
        {
            DBContext.Charts.Add(new Models.Chart() { Owner = "anna" });

            Tweet.Tweet reply = TestTweet(new Tweet.Tweet()
            {
                CreatorScreenName = "anna",
                Text = "title bob"
            });


            Assert.IsNull(reply.Image);
            Assert.AreEqual("bob", DBContext.Charts.First().Title);
            Assert.AreEqual(TitleCommand.ExistingChartTitleSet("bob"), reply.Text);
        }

        [TestMethod]
        public void SetTitle_ExistingUnnamedChart_WithData_MessageReturned()
        {
            DBContext.Charts.Add(new Models.Chart() { Id = 1, Owner= "anna" });
            DBContext.Points.Add(new Models.Point() { ChartId = 1, X = DateTime.Now, Y = 1 });
            DBContext.Points.Add(new Models.Point() { ChartId = 1, X = DateTime.Now.AddDays(1), Y = 2 });

            Tweet.Tweet reply = TestTweet(new Tweet.Tweet()
            {
                CreatorScreenName = "anna",
                Text = "title bob"
            });

            Assert.IsNotNull(reply.Image);
            Assert.AreEqual("bob", DBContext.Charts.First().Title);
            Assert.AreEqual(TitleCommand.ExistingChartTitleSet("bob"), reply.Text);
        }

        [TestMethod]
        public void SetTitle_ExistingNamedChart_MessageReturned()
        {
            DBContext.Charts.Add(new Models.Chart() { Title = "sue", Owner = "anna" });

            Tweet.Tweet reply = TestTweet(new Tweet.Tweet()
            {
                CreatorScreenName = "anna",
                Text = "title bob"
            });

            Assert.IsNull(reply.Image);
            Assert.AreEqual("bob", DBContext.Charts.First().Title);
            Assert.AreEqual(TitleCommand.ExistingChartTitleReset("sue", "bob"), reply.Text);
        }
    }
}
