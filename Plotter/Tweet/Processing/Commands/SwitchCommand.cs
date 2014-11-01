using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Plotter.Tweet.Processing.Commands
{
	public class SwitchCommand : CommandBase
	{

		protected override Tuple<byte[], string> GetReply(string[] commandParams)
		{
			Models.Chart chart = GetActiveChart();

			if(commandParams.Length == 0)
			{
				if(chart == null)
				{
					return new Tuple<byte[],string>(null, JustSendSomeDangData);
				}
			}

			string chartName = commandParams[0];

			//If chart is null, then they're adding their first chart
			if(chart == null)
			{
				if(!string.IsNullOrEmpty(chartName))
				{
					DBContext.Charts.Add(new Models.Chart(){
						Owner = this.UserScreenName,
						Title = chartName
					});
					DBContext.SaveChanges();
					//Create a new chart with this name
					return RenderResult(NewNamedChartCreated(chartName));
				}
				else
				{
					//they want to switch to a new, unnamed chart.
					DBContext.Charts.Add(new Models.Chart() { Owner = UserScreenName });
					DBContext.SaveChanges();
					return RenderResult(NewUnnamedChartCreated());
				}
			}
			else
			{
				//it isn't their first chart.
				//to continue, the active chart needs to have a name
				if(string.IsNullOrEmpty(chart.Title))
				{
                    return RenderResult(CurrentChartNeedsAName);
				}
				else
				{
                    var userCharts = DBContext.Charts.Where(c => c.Owner == UserScreenName).ToList();
                    var existing = userCharts.Where(c => c.Title.Equals(chartName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                    
                    foreach (var userChart in userCharts)
                        userChart.IsActive = false;

                    //there's already a chart by this name, switch to it
                    if(existing != null)
                    {
                        existing.IsActive = true;
                        DBContext.SaveChanges();
                        return RenderResult(SwitchedToPreExisting(existing.Title));
                    }
                    else
                    {
						//create new
                        DBContext.Charts.Add(new Models.Chart()
                        {
                            IsActive = true,
                            Owner = UserScreenName,
                            Title = chartName
                        });
                        DBContext.SaveChanges();

                        return RenderResult(NewNamedChartCreatedAndHowToViewPrevious(chartName, chart.Title));
	                }
				}
			}
		}



		public static string JustSendSomeDangData { get { return "Tweet me a number to add it to a chart!"; } }

        public static string CurrentChartNeedsAName { get { return "Before you can switch charts, give your current chart a title. Reply 'title <title>'"; } }

		public static string NewNamedChartCreated(string p)
		{
            return string.Format("Chart '{0}' created.", p);
		}

		public static string NewUnnamedChartCreated()
        {
            return string.Format("Chart created.");
		}

		public static string NewNamedChartCreatedAndHowToViewPrevious(string newName, string previousName)
        {
            return string.Format("Chart '{0}' created. To switch back to your previous chart reply 'switch {1}'", newName, previousName);
		}

		public static string SwitchedToPreExisting(string p)
		{
            return string.Format("Switched to chart '{0}'.", p);
		}
	}
}