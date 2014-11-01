using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Plotter.Tweet.Processing.Commands
{
    public class PingCommand : CommandBase
    {
        protected override Tuple<byte[],string> GetReply(string[] commandParams)
        {
            return new Tuple<byte[], string>(null, "pong");
        }
    }
}