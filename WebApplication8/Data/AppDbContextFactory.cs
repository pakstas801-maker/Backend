using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using WebApplication8.Data;

public class AppDbContextFactory
    : IDesignTimeDbContextFactory<AppDbContext>
{   
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=smart_booking_db;Username=postgres;Password=801208401352"
        );

        return new AppDbContext(optionsBuilder.Options);
    }
}
    