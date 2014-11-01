using Plotter.Models;
using Plotter.Tweet.Processing.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Plotter.Tweet.Processing
{
    public class TweetParser
    {
        private IPlotterDBContext _dbContext;

        public TweetParser(IPlotterDBContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public Tweet GetReply(Tweet incoming)
        {
            string commandText = incoming.Text.Replace("@plotpt", "").Trim();
            string[] cmdParts = commandText.Split(' ').Where(c => !string.IsNullOrWhiteSpace(c)).ToArray();
            
            ICommand command = GetCommand(cmdParts.Length == 0 ? "" : cmdParts[0]);
            command.SetDBContext(this._dbContext);

            Tweet response = command.GetResult(incoming.CreatorScreenName, cmdParts.Skip(1).ToArray());
            if(response != null)
            {
                return response;
            }

            return null;
        }

        /// <summary>
        /// Commands:
        /// ping (pong)
        /// new (starts a new chart)
        /// chart (renders chart)
        /// status - gets status
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private ICommand GetCommand(string cmd)
        {
            switch(cmd.ToLower())
            {
                case "ping":
                    return new PingCommand();
                case "title":
                    return new TitleCommand();
                case "switch":
                    return new SwitchCommand();
                default:
                    decimal d;
                    if (decimal.TryParse(cmd, out d))
                        return new AddPointCommand(d);
                    else
                        return new DefaultCommand();

            }
        }
    }
}