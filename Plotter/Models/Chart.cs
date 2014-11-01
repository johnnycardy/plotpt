using System;
using System.Collections.Generic;

namespace Plotter.Models
{
    public partial class Chart
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Owner { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
