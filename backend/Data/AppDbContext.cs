using System.ComponentModel;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<AppointmentCategory> AppointmentCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AppointmentCategory>()
                .HasKey(ac => new { ac.AppointmentId, ac.CategoryId });

            modelBuilder.Entity<AppointmentCategory>()
                .HasOne(ac => ac.Appointment)
                .WithMany(a => a.AppointmentCategories)
                .HasForeignKey(ac => ac.AppointmentId);

            modelBuilder.Entity<AppointmentCategory>()
                .HasOne(ac => ac.Category)
                .WithMany(c => c.AppointmentCategories)
                .HasForeignKey(ac => ac.CategoryId);
        }
    }
}