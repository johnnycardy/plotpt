﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Plotter.Tweet.Processing.Commands
{
    public class DefaultCommand : CommandBase
    {
        public static readonly string DefaultMessage = "I didn't understand that!";

        protected override Tuple<byte[], string> GetReply(string[] commandParams)
        {
            return RenderResult(DefaultMessage);
        }
    }
}