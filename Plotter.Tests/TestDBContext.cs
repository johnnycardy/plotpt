using Plotter.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plotter.Tests
{
    public class TestDBContext : IPlotterDBContext
    {
        public TestDBContext()
        {
            var charts = new TestDbSet<Chart>();
            var points = new TestDbSet<Point>();

            charts.Changed += DBChanged;
            points.Changed += DBChanged;

            Charts = charts;
            Points = points;
        }

        private void DBChanged(object sender, EventArgs e)
        {
            HasPendingChanges = true;
        }

        public bool HasPendingChanges { get; private set; }

        public DbSet<Chart> Charts
        {
            get; set;
        }

        public DbSet<Point> Points
        {
            get; set;
        }

        public int SaveChanges()
        {
            HasPendingChanges = false;
            return 0;   
        }

        public System.Data.Entity.Infrastructure.DbEntityEntry Entry(object entity)
        {
            return null;
        }

        public void Dispose()
        {
            
        }
    }
}
