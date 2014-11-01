using Plotter.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Plotter.Tweet.Processing
{
    public class QueueProcessor
    {
        private bool waiting = false;

        public QueueProcessor(ConcurrentQueue<Tweet> incomingQueue, IPlotterDBContext dbContext)
        {
            TweetParser parser = new TweetParser(dbContext);

            //Start processing the incoming queue
            ThreadPool.QueueUserWorkItem((t) =>
            {
                while(true)
                {
                    if (!waiting)
                    {
                        Tweet incoming;
                        if (incomingQueue.TryDequeue(out incoming))
                        {
                            //Process this tweet
                            Tweet response = parser.GetReply(incoming);
                            if(response != null)
                            {
                                OnTweetReady(response);
                            }
                        }
                    }

                    Thread.Sleep(200);
                }
            });
        }

        public event EventHandler<Tweet> TweetReady;

        private void OnTweetReady(Tweet tweet)
        {
            if(TweetReady != null)
            {
                waiting = true;
                TweetReady(this, tweet);
                waiting = false;
            }
        }
    }
}