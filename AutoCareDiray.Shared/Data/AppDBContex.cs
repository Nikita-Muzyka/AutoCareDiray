using AutoCareDiray.Shared.Models.Notes;
using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Models.VehicleModel;
using AutoCareDiray.Shared.Models.RefillModel;
using Microsoft.EntityFrameworkCore;
using System;

namespace AutoCareDiray.Shared.Data
{
    public class AppDBContex : DbContext
    {
        public AppDBContex(DbContextOptions<AppDBContex> options) : base(options)
        {
        }

        // Пустой конструктор (для миграций)
        public AppDBContex()
        {
        }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Repair> Repairs { get; set; }
        public DbSet<RepairType> RepairTypes { get; set; }
        public DbSet<VehicleNotes> VehicleNotes { get; set; }
        public DbSet<Refill> Refills { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Этот метод используется ТОЛЬКО для миграций (когда EF Tools запускают проект)
            if (!optionsBuilder.IsConfigured)
            {
                var dbPath = Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData), "vehiclesApp.db");

                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vehicle>()
                        .HasMany(v => v.Refills)
                        .WithOne(r => r.Vehicle)
                        .HasForeignKey(r => r.VehicleId)
                        .OnDelete(DeleteBehavior.Cascade); // 👈 Главная строчка

            // Настраиваем каскадное удаление для Ремонтов
            modelBuilder.Entity<Vehicle>()
                        .HasMany(v => v.Repairs)
                        .WithOne(r => r.Vehicle)
                        .HasForeignKey(r => r.VehicleId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Repair>()
                        .HasMany(r => r.SpareParts) 
                        .WithOne()                  
                        .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
