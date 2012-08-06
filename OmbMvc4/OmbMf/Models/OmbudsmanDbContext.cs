using System.Data.Entity;

namespace OmbMf.Models
{
    public class OmbudsmanDbContext : DbContext
    {
        public OmbudsmanDbContext()
            : base("Ombudsman")
        {
        }
#if needed
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Facility>().ToTable("Facilities");
            modelBuilder.Entity<Ombudsman>().ToTable("Ombudsmen");
            modelBuilder.Entity<Facility>().HasOptional<Ombudsman>(f => f.Ombudsman);

            // only supported with .NET 4.5 and VS 11
            modelBuilder.Entity<Facility>().Property(p => p.FacilityType).HasColumnName("FacilityType");
            System.Diagnostics.Debug.WriteLine("model created");
        }
#endif
        public DbSet<Ombudsman> Ombudsmen { get; set; }
        public DbSet<FacilityType> FacilityTypes { get; set; }
        public DbSet<Facility> Facilities { get; set; }
    }
}
