using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Plotter.Models.Mapping;

namespace Plotter.Models
{
    public partial class PlotterDBContext : DbContext
    {
        static PlotterDBContext()
        {
            Database.SetInitializer<PlotterDBContext>(null);
        }

        public PlotterDBContext()
            : base("Name=PlotterDBContext")
        {
        }

        public DbSet<Chart> Charts { get; set; }
        public DbSet<Point> Points { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ChartMap());
            modelBuilder.Configurations.Add(new PointMap());
        }
    }
}
