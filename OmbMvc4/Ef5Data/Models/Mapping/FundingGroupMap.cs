using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class FundingGroupMap : EntityTypeConfiguration<FundingGroup>
    {
        public FundingGroupMap()
        {
            // Primary Key
            this.HasKey(t => t.FundingGroupID);

            // Properties
            this.Property(t => t.FundingGroup1)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("FundingGroup");
            this.Property(t => t.FundingGroupID).HasColumnName("FundingGroupID");
            this.Property(t => t.FundingGroup1).HasColumnName("FundingGroup");
        }
    }
}
