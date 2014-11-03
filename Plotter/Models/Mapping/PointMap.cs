using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Plotter.Models.Mapping
{
    public class PointMap : EntityTypeConfiguration<Point>
    {
        public PointMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("Point");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ChartId).HasColumnName("ChartId");
            this.Property(t => t.Y).HasColumnName("Y");
            this.Property(t => t.X).HasColumnName("X");
        }
    }
}
