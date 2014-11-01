using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plotter.Tweet;
using Plotter.Tweet.Processing;
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
    public class PingPongTests : TestBase
    {
        [TestMethod]
        public void PingPongTest()
        {
            var result = TestTweet(new Tweet.Tweet()
            {
                CreatorScreenName = "anna",
                Text = "ping"
            });
                
            Assert.AreEqual("@anna pong", result.GetMessageForSending());
        }
    }
}
