using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class FundingTypeMap : EntityTypeConfiguration<FundingType>
    {
        public FundingTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.FundingTypeID);

            // Properties
            this.Property(t => t.FundingType1)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Code)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("FundingType");
            this.Property(t => t.FundingTypeID).HasColumnName("FundingTypeID");
            this.Property(t => t.FundingType1).HasColumnName("FundingType");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.FundingGroupID).HasColumnName("FundingGroupID");
            this.Property(t => t.SourceAgencyID).HasColumnName("SourceAgencyID");
            this.Property(t => t.RecipientAgencyID).HasColumnName("RecipientAgencyID");
            this.Property(t => t.Discretion).HasColumnName("Discretion");
            this.Property(t => t.ConformityImpact).HasColumnName("ConformityImpact");
            this.Property(t => t.RankOrder).HasColumnName("RankOrder");

            // Relationships
            this.HasOptional(t => t.FundingGroup)
                .WithMany(t => t.FundingTypes)
                .HasForeignKey(d => d.FundingGroupID);
            this.HasOptional(t => t.Organization)
                .WithMany(t => t.FundingTypes)
                .HasForeignKey(d => d.RecipientAgencyID);

        }
    }
}
