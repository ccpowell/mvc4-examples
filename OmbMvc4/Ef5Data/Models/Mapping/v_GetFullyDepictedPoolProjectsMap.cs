using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class v_GetFullyDepictedPoolProjectsMap : EntityTypeConfiguration<v_GetFullyDepictedPoolProjects>
    {
        public v_GetFullyDepictedPoolProjectsMap()
        {
            // Primary Key
            this.HasKey(t => new { t.COGID, t.ProjectVersionID, t.TIPID, t.TIP_Year });

            // Properties
            this.Property(t => t.COGID)
                .IsRequired()
                .HasMaxLength(25);

            this.Property(t => t.ProjectVersionID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.FacilityName)
                .HasMaxLength(75);

            this.Property(t => t.ProjectName)
                .HasMaxLength(100);

            this.Property(t => t.TIPID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.TIP_Year)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Version_Status)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("v_GetFullyDepictedPoolProjects");
            this.Property(t => t.COGID).HasColumnName("COGID");
            this.Property(t => t.ProjectVersionID).HasColumnName("ProjectVersionID");
            this.Property(t => t.PoolMasterVersionID).HasColumnName("PoolMasterVersionID");
            this.Property(t => t.FacilityName).HasColumnName("FacilityName");
            this.Property(t => t.ProjectName).HasColumnName("ProjectName");
            this.Property(t => t.TIPID).HasColumnName("TIPID");
            this.Property(t => t.TIP_Year).HasColumnName("TIP Year");
            this.Property(t => t.Version_Status).HasColumnName("Version Status");
        }
    }
}
