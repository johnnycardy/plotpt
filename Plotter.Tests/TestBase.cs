using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plotter.Tweet.Processing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Plotter.Tests
{
    public class TestBase
    {
        protected TestDBContext DBContext { get; set; }

        [TestInitialize]
        public void SetUpTest()
        {
            DBContext = new TestDBContext();
        }

        [TestCleanup]
        public void TearDownTest()
        {
            Assert.IsFalse(DBContext.HasPendingChanges, "Database should not have pending changes at the end of a test.");
        }

        protected Tweet.Tweet TestTweet(Tweet.Tweet input)
        {
            Tweet.Tweet result = null;

            ConcurrentQueue<Tweet.Tweet> queue = new ConcurrentQueue<Tweet.Tweet>();
            QueueProcessor qp = new QueueProcessor(queue, DBContext);

            qp.TweetReady += (sender, e) =>
            {
                result = e;
            };

            Task t = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Thread.Sleep(100);
                    if (result != null)
                        return;
                }
            });


            queue.Enqueue(input);
            t.Wait();

            return result;
        }
    }
}
