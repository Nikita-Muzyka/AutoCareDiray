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

        }
    }
}
