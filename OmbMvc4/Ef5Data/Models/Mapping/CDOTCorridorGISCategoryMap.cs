using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class CDOTCorridorGISCategoryMap : EntityTypeConfiguration<CDOTCorridorGISCategory>
    {
        public CDOTCorridorGISCategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.CDOTCorridorGISCategoryID);

            // Properties
            this.Property(t => t.CDOTCorridorGISCategoryID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("CDOTCorridorGISCategory");
            this.Property(t => t.CDOTCorridorGISCategoryID).HasColumnName("CDOTCorridorGISCategoryID");
            this.Property(t => t.RouteID).HasColumnName("RouteID");
        }
    }
}
