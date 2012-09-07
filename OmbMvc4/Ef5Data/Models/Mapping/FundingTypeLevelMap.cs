using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class FundingTypeLevelMap : EntityTypeConfiguration<FundingTypeLevel>
    {
        public FundingTypeLevelMap()
        {
            // Primary Key
            this.HasKey(t => new { t.FundingTypeID, t.FundingLevelID, t.IsActive });

            // Properties
            this.Property(t => t.FundingTypeID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.FundingLevelID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("FundingTypeLevel");
            this.Property(t => t.FundingTypeID).HasColumnName("FundingTypeID");
            this.Property(t => t.FundingLevelID).HasColumnName("FundingLevelID");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
        }
    }
}
