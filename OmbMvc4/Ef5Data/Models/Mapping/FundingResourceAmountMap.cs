using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class FundingResourceAmountMap : EntityTypeConfiguration<FundingResourceAmount>
    {
        public FundingResourceAmountMap()
        {
            // Primary Key
            this.HasKey(t => t.FundingResourceAmountID);

            // Properties
            // Table & Column Mappings
            this.ToTable("FundingResourceAmount");
            this.Property(t => t.FundingResourceAmountID).HasColumnName("FundingResourceAmountID");
            this.Property(t => t.FundingResourceID).HasColumnName("FundingResourceID");
            this.Property(t => t.FundingIncrementID).HasColumnName("FundingIncrementID");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.StateWideAmount).HasColumnName("StateWideAmount");

            // Relationships
            this.HasRequired(t => t.FundingIncrement)
                .WithMany(t => t.FundingResourceAmounts)
                .HasForeignKey(d => d.FundingIncrementID);

        }
    }
}
