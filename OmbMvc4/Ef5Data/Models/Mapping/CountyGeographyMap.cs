using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class CountyGeographyMap : EntityTypeConfiguration<CountyGeography>
    {
        public CountyGeographyMap()
        {
            // Primary Key
            this.HasKey(t => t.CountyGeographyID);

            // Properties
            this.Property(t => t.CountyGeographyID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.FIPSCode)
                .HasMaxLength(10);

            this.Property(t => t.StateCode)
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("CountyGeography");
            this.Property(t => t.CountyGeographyID).HasColumnName("CountyGeographyID");
            this.Property(t => t.FIPSCode).HasColumnName("FIPSCode");
            this.Property(t => t.StateCode).HasColumnName("StateCode");

            // Relationships
            this.HasRequired(t => t.Geography)
                .WithOptional(t => t.CountyGeography);

        }
    }
}
