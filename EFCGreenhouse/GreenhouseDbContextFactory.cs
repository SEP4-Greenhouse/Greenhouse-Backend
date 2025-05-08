using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EFCGreenhouse
{
    public class GreenhouseDbContextFactory : IDesignTimeDbContextFactory<GreenhouseDbContext>
    {
        public GreenhouseDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GreenhouseDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=greenhouse;Username=postgres;Password=postgres");

            return new GreenhouseDbContext(optionsBuilder.Options);
        }
    }
}