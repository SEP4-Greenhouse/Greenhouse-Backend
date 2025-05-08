using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EFCGreenhouse
{
    public class GreenhouseDbContextFactory : IDesignTimeDbContextFactory<GreenhouseDbContext>
    {
        public GreenhouseDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GreenhouseDbContext>();
            var connectionString = Environment.GetEnvironmentVariable("AIVEN_DB_CONNECTION");
            optionsBuilder.UseNpgsql(connectionString);
            return new GreenhouseDbContext(optionsBuilder.Options);
        }
    }
}