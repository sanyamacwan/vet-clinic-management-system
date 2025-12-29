using Microsoft.EntityFrameworkCore;

namespace VetClinicSystem.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<VetDoctor> VetDoctors { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<PetProfile> PetProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1 doctor → many pets
            modelBuilder.Entity<VetDoctor>()
                .HasMany(d => d.Pets)
                .WithOne(p => p.VetDoctor)
                .HasForeignKey(p => p.VetDoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // 1 pet → 1 pet profile
            modelBuilder.Entity<Pet>()
                .HasOne(p => p.PetProfile)
                .WithOne(pp => pp.Pet)
                .HasForeignKey<PetProfile>(pp => pp.PetId);
        }
    }
}
