using CarMaintenanceTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace CarMaintenanceTracker.Data;

public class AppDbContext : DbContext
{
    public const string DatabaseFileName = "carmaintenance.db";

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<ServiceEntry> ServiceEntries => Set<ServiceEntry>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vehicle>()
            .HasMany(v => v.ServiceEntries)
            .WithOne(e => e.Vehicle)
            .HasForeignKey(e => e.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
