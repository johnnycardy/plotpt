using Plotter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plotter.Tweet.Processing.Commands
{
    interface ICommand
    {
        Tweet GetResult(string fromUser, string[] command);

        void SetDBContext(IPlotterDBContext dbContext);
    }
}
