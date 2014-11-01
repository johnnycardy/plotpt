using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plotter.Tweet;
using Plotter.Tweet.Processing;
using Plotter.Tweet.Processing.Commands;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Plotter.Tests
{
    [TestClass]
    public class DefaultTests : TestBase
    {
        [TestMethod]
        public void DefaultCommandTest_Empty()
        {
            var result = TestTweet(new Tweet.Tweet()
            {
                CreatorScreenName = "anna",
                Text = ""
            });

            Assert.AreEqual("@anna " + DefaultCommand.DefaultMessage, result.GetMessageForSending());
        }

        [TestMethod]
        public void DefaultCommandTest_Nonsense()
        {
            var result = TestTweet(new Tweet.Tweet()
            {
                CreatorScreenName = "anna",
                Text = "dzjfhuhf"
            });

            Assert.AreEqual("@anna " + DefaultCommand.DefaultMessage, result.GetMessageForSending());
        }
    }
}
