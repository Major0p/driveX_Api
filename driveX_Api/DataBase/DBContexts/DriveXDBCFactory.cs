using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace driveX_Api.DataBase.DBContexts
{
    public class DriveXDBCFactory : IDesignTimeDbContextFactory<DriveXDBC>
    {
        public DriveXDBC CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

            var connectionString = config.GetConnectionString("driveX");
            var optionsBuilder = new DbContextOptionsBuilder<DriveXDBC>();

            optionsBuilder.UseSqlServer(connectionString);

            return new DriveXDBC(optionsBuilder.Options);
        }
    }
}
