using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EFCGreenhouse
{
    public class GreenhouseDbContextFactory : IDesignTimeDbContextFactory<GreenhouseDbContext>
    {
        public GreenhouseDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GreenhouseDbContext>();
            optionsBuilder.UseSqlite("Data Source=greenhouse.db");

            return new GreenhouseDbContext(optionsBuilder.Options);
        }
    }
}