using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class FundingLevelMap : EntityTypeConfiguration<FundingLevel>
    {
        public FundingLevelMap()
        {
            // Primary Key
            this.HasKey(t => t.FundingLevelID);

            // Properties
            this.Property(t => t.FundingLevel1)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("FundingLevel");
            this.Property(t => t.FundingLevelID).HasColumnName("FundingLevelID");
            this.Property(t => t.FundingLevel1).HasColumnName("FundingLevel");
        }
    }
}
