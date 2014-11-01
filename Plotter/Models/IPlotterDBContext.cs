using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plotter.Models
{
    public interface IPlotterDBContext
    {

        DbSet<Chart> Charts { get; set; }
        DbSet<Point> Points { get; set; }

        int SaveChanges();
        DbEntityEntry Entry(object entity);
        void Dispose();
    }
}
