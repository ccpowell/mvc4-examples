using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class TIPProjectEvaluation2010ServiceTypeMap : EntityTypeConfiguration<TIPProjectEvaluation2010ServiceType>
    {
        public TIPProjectEvaluation2010ServiceTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.ServiceTypeID);

            // Properties
            this.Property(t => t.ServiceType)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TIPProjectEvaluation2010ServiceType");
            this.Property(t => t.ServiceTypeID).HasColumnName("ServiceTypeID");
            this.Property(t => t.ServiceType).HasColumnName("ServiceType");
        }
    }
}
