using JassApp.Common.Settings.Provisioning.Models;
using JassApp.DataAccess.DbContexts.Contexts.Implementation;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace JassApp.DataAccess.DbContexts.Factories.Implementation
{
    // Used by EF Core Code First and EF Core Power Tools
    // This will never be called in running code
    [UsedImplicitly]
    public class DesignTimeAppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddUserSecrets<AppSettings>();

            var config = configBuilder.Build();
            var connectionString = config["AppSettings:ConnectionString"];

            var options = new DbContextOptionsBuilder()
                .UseSqlServer(connectionString)
                .Options;

            return new AppDbContext(options);
        }
    }
}