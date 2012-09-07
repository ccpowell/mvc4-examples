using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class FundingResourceMap : EntityTypeConfiguration<FundingResource>
    {
        public FundingResourceMap()
        {
            // Primary Key
            this.HasKey(t => t.FundingResourceID);

            // Properties
            // Table & Column Mappings
            this.ToTable("FundingResource");
            this.Property(t => t.FundingResourceID).HasColumnName("FundingResourceID");
            this.Property(t => t.FundingTypeID).HasColumnName("FundingTypeID");
            this.Property(t => t.ProgramID).HasColumnName("ProgramID");
            this.Property(t => t.FundPeriodID).HasColumnName("FundPeriodID");
            this.Property(t => t.Temp_PreviousResourceID).HasColumnName("Temp_PreviousResourceID");

            // Relationships
            this.HasRequired(t => t.FundingType)
                .WithMany(t => t.FundingResources)
                .HasForeignKey(d => d.FundingTypeID);
            this.HasRequired(t => t.ProgramInstance)
                .WithMany(t => t.FundingResources)
                .HasForeignKey(d => new { d.ProgramID, d.FundPeriodID });

        }
    }
}
