using Plotter.Models;
using Plotter.Tweet.Processing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Tweetinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Streaminvi;

namespace Plotter.Tweet
{
    public class TweetIO
    {
        private static object _lockObj = new object();
        private static TweetIO _instance = null;

        public static bool IsAwake
        {
            get
            {
                return _instance != null;
            }
        }

        public static void WakeUp()
        {
            if(_instance == null)
            {
                lock(_lockObj)
                {
                    if(_instance == null)
                    {
                        _instance = new TweetIO();
                    }
                }
            }
        }

        private TweetIO()
        {
            string userAccessToken = System.Configuration.ConfigurationManager.AppSettings["userAccessToken"];
            string userAccessSecret = System.Configuration.ConfigurationManager.AppSettings["userAccessSecret"];
            string consumerKey = System.Configuration.ConfigurationManager.AppSettings["consumerKey"];
            string consumerSecret = System.Configuration.ConfigurationManager.AppSettings["consumerSecret"];
            
            TwitterCredentials.SetCredentials(userAccessToken, userAccessSecret, consumerKey, consumerSecret);
            
            ConcurrentQueue<Tweet> incomingQueue = new ConcurrentQueue<Tweet>();
            QueueProcessor processor = new QueueProcessor(incomingQueue, new PlotterDBContext());

            StartHandleOutgoing(processor);
            StartHandleIncoming(incomingQueue);
        }

        private void StartHandleIncoming(ConcurrentQueue<Tweet> queue)
        {
            var us = Stream.CreateUserStream();
            us.TweetCreatedByAnyone += (sender, args) =>
            {
                var userMentions = args.Tweet.UserMentions;
                if (userMentions != null && userMentions.Any(x => x.ScreenName == "plotpt"))
                {
                    queue.Enqueue(new Tweet(args.Tweet));
                }
            };
            var thread2 = new Thread(() => us.StartStream());
            thread2.Start();
        }

        private void StartHandleOutgoing(QueueProcessor queueProcessor)
        {
            queueProcessor.TweetReady += (object sender, Tweet e) =>
            {
                ITweet tweet = null;
                
                if(e.Image != null && e.Image.Length > 0)
                {
                    tweet = Tweetinvi.Tweet.CreateTweetWithMedia(e.GetMessageForSending(), e.Image);
                }
                else
                {
                    tweet = Tweetinvi.Tweet.CreateTweet(e.GetMessageForSending());
                }

                try
                {
                    bool success = tweet.Publish();
                }
                catch(Exception ex)
                {

                }
            };
        }
    }
}