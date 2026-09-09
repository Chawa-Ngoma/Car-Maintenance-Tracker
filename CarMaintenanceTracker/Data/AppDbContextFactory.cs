using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CarMaintenanceTracker.Data;

/// <summary>
/// Lets `dotnet ef` create an AppDbContext at design time, since a WPF app has
/// no ASP.NET-style host to resolve options from.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite($"Data Source={AppDbContext.DatabaseFileName}");
        return new AppDbContext(optionsBuilder.Options);
    }
}
