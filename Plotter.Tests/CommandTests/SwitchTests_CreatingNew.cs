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
    public class SwitchTests_CreatingNew : TestBase
    {

        [TestMethod]
        public void SwitchChart_NonePreExisting_MessageReturned()
        {
            Tweet.Tweet reply = TestTweet(new Tweet.Tweet()
            {
                CreatorScreenName = "anna",
                Text = "switch"
            });

            Assert.IsNull(reply.Image);
            Assert.AreEqual(SwitchCommand.JustSendSomeDangData, reply.Text);
        }

        [TestMethod]
        public void SwitchChart_CreateNew_WithName_NonePreExisting_MessageReturned()
        {
            Tweet.Tweet reply = TestTweet(new Tweet.Tweet()
            {
                CreatorScreenName = "anna",
                Text = "switch bob"
            });

            Assert.IsNull(reply.Image);
            Assert.AreEqual("bob", DBContext.Charts.First().Title);
            Assert.AreEqual(SwitchCommand.NewNamedChartCreated("bob"), reply.Text);
        }

        [TestMethod]
        public void SwitchChart_CreateNew_WithPreExisting_MessageReturned()
        {
            DBContext.Charts.Add(new Models.Chart() { Owner = "anna", Title = "chartA" });

            Tweet.Tweet reply = TestTweet(new Tweet.Tweet()
            {
                CreatorScreenName = "anna",
                Text = "switch bob"
            });

            Assert.IsNull(reply.Image);
            Assert.AreEqual(SwitchCommand.NewNamedChartCreatedAndHowToViewPrevious("bob", "chartA"), reply.Text);
        }

        [TestMethod]
        public void SwitchChart_CreateNew_WithPreExisting_Unnamed_MessageReturned()
        {
            DBContext.Charts.Add(new Models.Chart() { Owner = "anna" });
            DBContext.SaveChanges();
            Tweet.Tweet reply = TestTweet(new Tweet.Tweet()
            {
                CreatorScreenName = "anna",
                Text = "switch bob"
            });

            Assert.IsNull(reply.Image);
            Assert.AreEqual(SwitchCommand.CurrentChartNeedsAName, reply.Text);
        }
    }
}