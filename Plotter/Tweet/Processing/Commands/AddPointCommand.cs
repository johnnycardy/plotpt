using Plotter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Plotter.Tweet.Processing.Commands
{
    public class AddPointCommand : CommandBase
    {
        public static readonly string VeryFirstPoint_ChartCreated = "Chart created! Send some more numbers to get a chart image back.";

        public static readonly string FirstPoint_ExistingChart = "Send some more numbers to get a chart image back.";

        public static readonly string SecondPoint_ExistingChart_Titled = "Keep sending numbers! To start a new chart at any time, reply 'switch'. Reply 'title' to give this chart a name.";

        public static readonly string SecondPoint_ExistingChart_Untitled = "Keep sending numbers! To start a new chart at any time, reply 'switch'. Reply 'title' to give this chart a name.";

        private decimal d;

        public AddPointCommand(decimal d)
        {
            this.d = d;
        }

        /// n (adds number to chart)
        ///     - if first point, "chart created. send more data to see chart"
        ///     - if >second point, replies with chart
        protected override Tuple<byte[], string> GetReply(string[] commandParams)
        {
            bool chartCreated = false;

            Models.Chart chart = GetActiveChart();

            if (chart == null)
            {
                chart = new Models.Chart() { Owner = UserScreenName, Title = "" };
                DBContext.Charts.Add(chart);
                DBContext.SaveChanges();
                chartCreated = true;
            }

            var addedPoints = AddPoints(chart, DateTime.UtcNow, this.d);
            var allPoints = GetPoints(chart);
            string message = "";

            if(chartCreated)
            {
                if (allPoints.Count() == 1)
                {
                    return RenderResult(VeryFirstPoint_ChartCreated);
                }
            }
            else
            {
                if (allPoints.Count() == 1)
                {
                    return RenderResult(FirstPoint_ExistingChart);
                }
                else if(allPoints.Count() == 2)
                {
                    if (string.IsNullOrEmpty(chart.Title))
                    {
                        return RenderResult(SecondPoint_ExistingChart_Untitled, chart, allPoints);
                    }
                    else
                    {
                        return RenderResult(SecondPoint_ExistingChart_Titled, chart, allPoints);
                    }
                }
            }

            return RenderResult(message, chart, allPoints);
        }

        private List<Models.Point> AddPoints(Models.Chart chart, DateTime dateTime, decimal pt1)
        {
            List<Models.Point> result = new List<Models.Point>();

            result.Add(new Models.Point()
            {
                ChartId = chart.Id,
                X = dateTime,
                Y = this.d
            });

            foreach(Models.Point pt in result)
            {
                DBContext.Points.Add(pt);
            }
            DBContext.SaveChanges();
            return result;
        }
    }
}