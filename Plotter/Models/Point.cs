using System;
using System.Collections.Generic;

namespace Plotter.Models
{
    public partial class Point
    {
        public int Id { get; set; }
        public int ChartId { get; set; }
        public decimal Y { get; set; }
        public System.DateTime X { get; set; }
    }
}
