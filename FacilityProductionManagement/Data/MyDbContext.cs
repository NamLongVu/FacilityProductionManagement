using FacilityProductionManagement.Model;
using Microsoft.EntityFrameworkCore;

namespace FacilityProductionManagement.Data
{
    public class MyDbContext : DbContext
    {
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<EquipmentContract> EquipmentContracts { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EquipmentContract>()
            .HasKey(c => c.Id);

            modelBuilder.Entity<EquipmentContract>()
                .HasOne(e => e.Facility)
                .WithMany(p => p.EquipmentContracts)
                .HasForeignKey(e => e.FacilityCode)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EquipmentContract>()
                .HasOne(e => e.EquipmentType)
                .WithMany(p => p.EquipmentContracts)
                .HasForeignKey(e => e.EquipmentTypeCode)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
