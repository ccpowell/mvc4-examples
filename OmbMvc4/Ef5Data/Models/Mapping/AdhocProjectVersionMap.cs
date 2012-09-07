using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class AdhocProjectVersionMap : EntityTypeConfiguration<AdhocProjectVersion>
    {
        public AdhocProjectVersionMap()
        {
            // Primary Key
            this.HasKey(t => t.AdhocProjectVersionID);

            // Properties
            this.Property(t => t.AdhocProjectVersionID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("AdhocProjectVersion");
            this.Property(t => t.AdhocProjectVersionID).HasColumnName("AdhocProjectVersionID");
        }
    }
}
