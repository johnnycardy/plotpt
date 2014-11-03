using Plotter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Plotter.Tweet.Processing.Commands
{
    public abstract class CommandBase : ICommand
    {
        protected abstract Tuple<byte[],string> GetReply(string[] commandParams);

        protected string UserScreenName { get; private set; }

        public Tweet GetResult(string fromUser, string[] command)
        {
            UserScreenName = fromUser;

            Tuple<byte[], string> reply = GetReply(command);
            if(reply != null)
            {
                return new Tweet(fromUser, reply.Item2, reply.Item1);
            }
            else
            {
                return null;
            }
        }

        public void SetDBContext(IPlotterDBContext dbContext)
        {
            DBContext = dbContext;
        }

        protected IPlotterDBContext DBContext { get; private set; }

        protected Chart GetActiveChart()
        {
            return DBContext.Charts.OrderByDescending(c => c.IsActive).ThenByDescending(c => c.Id).FirstOrDefault(c => c.Owner == UserScreenName);
        }

        protected IEnumerable<Point> GetPoints(Chart chart)
        {
            return DBContext.Points.Where(p => p.ChartId == chart.Id).OrderBy(p => p.X);
        }

        protected Tuple<byte[], string> RenderResult(string message, Chart chart = null, IEnumerable<Point> allPoints = null)
        {
            if (chart == null)
                chart = GetActiveChart();

            if (allPoints == null && chart != null)
                allPoints = GetPoints(chart);

            byte[] chartImage = null;

            //Only render the chart if there's a few points.
            if (allPoints != null && allPoints.Count() > 1)
            {
                //Render the chart and return with the message
                var rendering = new Rendering(chart, allPoints.ToArray());
                chartImage = rendering.RenderPng();
            }

            return new Tuple<byte[], string>(chartImage, message);
        }
    }
}