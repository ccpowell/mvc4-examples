using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class GetFundingResourceMap : EntityTypeConfiguration<GetFundingResource>
    {
        public GetFundingResourceMap()
        {
            // Primary Key
            this.HasKey(t => new { t.FundingType, t.TimePeriod, t.Program });

            // Properties
            this.Property(t => t.FundingType)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Code)
                .HasMaxLength(50);

            this.Property(t => t.SourceAgency)
                .HasMaxLength(75);

            this.Property(t => t.RecipientAgency)
                .HasMaxLength(75);

            this.Property(t => t.TimePeriod)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Program)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.FundingGroup)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("GetFundingResources");
            this.Property(t => t.FundingType).HasColumnName("FundingType");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.SourceAgency).HasColumnName("SourceAgency");
            this.Property(t => t.RecipientAgency).HasColumnName("RecipientAgency");
            this.Property(t => t.Discretion).HasColumnName("Discretion");
            this.Property(t => t.TimePeriod).HasColumnName("TimePeriod");
            this.Property(t => t.Program).HasColumnName("Program");
            this.Property(t => t.FundingGroup).HasColumnName("FundingGroup");
        }
    }
}
