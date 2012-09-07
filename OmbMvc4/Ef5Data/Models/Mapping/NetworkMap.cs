using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class NetworkMap : EntityTypeConfiguration<Network>
    {
        public NetworkMap()
        {
            // Primary Key
            this.HasKey(t => t.NetworkID);

            // Properties
            this.Property(t => t.Network1)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Staging)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Network");
            this.Property(t => t.NetworkID).HasColumnName("NetworkID");
            this.Property(t => t.Network1).HasColumnName("Network");
            this.Property(t => t.cycleId).HasColumnName("cycleId");
            this.Property(t => t.networkGuid).HasColumnName("networkGuid");
            this.Property(t => t.Staging).HasColumnName("Staging");

            // Relationships
            this.HasOptional(t => t.Cycle)
                .WithMany(t => t.Networks)
                .HasForeignKey(d => d.cycleId);

        }
    }
}
