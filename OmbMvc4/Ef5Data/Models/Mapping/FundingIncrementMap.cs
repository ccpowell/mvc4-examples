using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class FundingIncrementMap : EntityTypeConfiguration<FundingIncrement>
    {
        public FundingIncrementMap()
        {
            // Primary Key
            this.HasKey(t => t.FundingIncrementID);

            // Properties
            this.Property(t => t.FundingIncrement1)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("FundingIncrement");
            this.Property(t => t.FundingIncrementID).HasColumnName("FundingIncrementID");
            this.Property(t => t.FundingIncrement1).HasColumnName("FundingIncrement");
            this.Property(t => t.BaseYearModifier).HasColumnName("BaseYearModifier");
            this.Property(t => t.EndYearModifier).HasColumnName("EndYearModifier");
            this.Property(t => t.ListOrder).HasColumnName("ListOrder");

            // Relationships
            this.HasMany(t => t.TimePeriods)
                .WithMany(t => t.FundingIncrements)
                .Map(m =>
                    {
                        m.ToTable("TimePeriodFundingIncrement");
                        m.MapLeftKey("FundingIncrementId");
                        m.MapRightKey("TimePeriodId");
                    });


        }
    }
}
