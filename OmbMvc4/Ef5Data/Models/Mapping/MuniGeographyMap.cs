using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class MuniGeographyMap : EntityTypeConfiguration<MuniGeography>
    {
        public MuniGeographyMap()
        {
            // Primary Key
            this.HasKey(t => t.MuniGeographyID);

            // Properties
            this.Property(t => t.MuniGeographyID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.PlaceCode)
                .HasMaxLength(10);

            this.Property(t => t.FIPS)
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("MuniGeography");
            this.Property(t => t.MuniGeographyID).HasColumnName("MuniGeographyID");
            this.Property(t => t.PlaceCode).HasColumnName("PlaceCode");
            this.Property(t => t.FIPS).HasColumnName("FIPS");
            this.Property(t => t.MuniTypeID).HasColumnName("MuniTypeID");

            // Relationships
            this.HasRequired(t => t.Geography)
                .WithOptional(t => t.MuniGeography);
            this.HasOptional(t => t.GeographyType)
                .WithMany(t => t.MuniGeographies)
                .HasForeignKey(d => d.MuniTypeID);

        }
    }
}
