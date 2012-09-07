using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class FundingResourceTimePeriodMap : EntityTypeConfiguration<FundingResourceTimePeriod>
    {
        public FundingResourceTimePeriodMap()
        {
            // Primary Key
            this.HasKey(t => new { t.FundingResourceId, t.TimePeriodId });

            // Properties
            this.Property(t => t.FundingResourceId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TimePeriodId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("FundingResourceTimePeriod");
            this.Property(t => t.FundingResourceId).HasColumnName("FundingResourceId");
            this.Property(t => t.TimePeriodId).HasColumnName("TimePeriodId");
        }
    }
}
