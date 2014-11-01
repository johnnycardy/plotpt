using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tweetinvi.Core.Interfaces;

namespace Plotter.Tweet
{
    public class Tweet
    {
        public string CreatorScreenName { get; set; }
        public string RecipientScreenName { get; set; }
        public string Text { get; set; }
        public byte[] Image { get; set; }

        public Tweet()
        {

        }

        public Tweet(string toScreenName, string text, byte[] image)
        {
            RecipientScreenName = toScreenName;
            Text = text;
            Image = image;

            if(GetMessageForSending().Length > 140)
            {
                throw new InvalidOperationException("Exceeded max tweet length!");
            }
        }

        public Tweet(ITweet tweet)
        {
            CreatorScreenName = tweet.Creator.ScreenName;
            Text = tweet.Text;
        }

        public string GetMessageForSending()
        {
            if (string.IsNullOrEmpty(Text))
            {
                return string.Format("@{0}", RecipientScreenName);
            }
            else
            {
                return string.Format("@{0} {1}", RecipientScreenName, Text);
            }
        }
    }

    public class TweetEventArgs : EventArgs
    {
        public Tweet Tweet { get; set; }
    }
}