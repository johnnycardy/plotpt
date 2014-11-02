using Plotter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Plotter.Tweet.Processing.Commands
{
    public class TitleCommand : CommandBase
    {
        protected override Tuple<byte[], string> GetReply(string[] commandParams)
        {
            if (commandParams.Length == 0)
            {
                return new Tuple<byte[], string>(null, InvalidCommandMessage);
            }
            else
            {
                string result = null;
                string title = commandParams[0];
                Chart c = GetActiveChart();
                if (c == null)
                {
                    c = new Chart();
                    DBContext.Charts.Add(c);
                    result = NewChartTitleSet(title);
                }
                else
                {
                    if (string.IsNullOrEmpty(c.Title))
                    {
                        result = ExistingChartTitleSet(title);
                    }
                    else
                    {
                        result = ExistingChartTitleReset(c.Title, title);
                    }
                }

                c.Title = title;

                DBContext.SaveChanges();

                return RenderResult(result, c);
            }
        }

        public static string InvalidCommandMessage
        {
            get { return "No new title supplied! Reply 'title newtitle' to set a title."; }
        }

        public static string NewChartTitleSet(string title)
        {
            return string.Format("New chart created with title \"{0}\". Tweet some numbers to get an image back!", title);
        }

        public static string ExistingChartTitleSet(string title)
        {
            return string.Format("Chart title set to \"{0}\".", title);
        }

        public static string ExistingChartTitleReset(string oldTitle, string newTitle)
        {
            return string.Format("Chart title changed from \"{0}\" to \"{1}\".", oldTitle, newTitle);
        }
    }
}