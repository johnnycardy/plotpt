﻿using Plotter.Models;
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
        
        public TweetIO()
        {
            
            string userAccessToken = System.Configuration.ConfigurationManager.AppSettings["userAccessToken"];
            string userAccessSecret = System.Configuration.ConfigurationManager.AppSettings["userAccessSecret"];
            string consumerKey = System.Configuration.ConfigurationManager.AppSettings["consumerKey"];
            string consumerSecret = System.Configuration.ConfigurationManager.AppSettings["consumerSecret"];
            
            TwitterCredentials.SetCredentials(userAccessToken, userAccessSecret, consumerKey, consumerSecret);

            PlotterDBContext dbContext = new PlotterDBContext();

            ConcurrentQueue<Tweet> incomingQueue = new ConcurrentQueue<Tweet>();
            QueueProcessor processor = new QueueProcessor(incomingQueue, dbContext);

            StartHandleOutgoing(processor);
            StartHandleIncoming(incomingQueue);
        }

        private void StartHandleIncoming(ConcurrentQueue<Tweet> queue)
        {
            /*
            var thread = new Thread(() =>
            {
                var fs = Stream.CreateFilteredStream();
                fs.AddTrack("@plotpt");
                fs.AddTrack("plotpt");
                fs.MatchingTweetReceived += (sender, args) =>
                {

                };
                fs.StartStreamMatchingAnyCondition();
            });
            thread.Start();
            */
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
                ITweet tweet = Tweetinvi.Tweet.CreateTweet(e.GetMessageForSending());
                tweet.Publish();
            };
        }
    }
}