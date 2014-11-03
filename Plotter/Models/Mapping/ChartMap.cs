using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Plotter.Models.Mapping
{
    public class ChartMap : EntityTypeConfiguration<Chart>
    {
        public ChartMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Title)
                .HasMaxLength(50);

            this.Property(t => t.Owner)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Chart");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Owner).HasColumnName("Owner");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
        }
    }
}
