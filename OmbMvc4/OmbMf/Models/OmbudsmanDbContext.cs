using System.Data.Entity;

namespace OmbMf.Models
{
    public class OmbudsmanDbContext : DbContext
    {
        public OmbudsmanDbContext()
            : base("Ombudsman")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
#if needed
            modelBuilder.Entity<Facility>().ToTable("Facilities");
            modelBuilder.Entity<Ombudsman>().ToTable("Ombudsmen");
            modelBuilder.Entity<Facility>().HasOptional<Ombudsman>(f => f.Ombudsman);
            System.Diagnostics.Debug.WriteLine("model created");
#endif
        }
        public DbSet<Ombudsman> Ombudsmen { get; set; }
        public DbSet<Facility> Facilities { get; set; }
    }
}
