using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace JassApp.DataAccess.DbContexts.Factories.Implementation
{
    [UsedImplicitly]
    public class DbContextOptionsFactory : IDbContextOptionsFactory
    {
        public DbContextOptions CreateForSqlServer(string connectionString)
        {
            var configuration = SqlServerConventionSetBuilder.Build();
            var mb = new ModelBuilder(configuration);
            mb.ApplyConfigurationsFromAssembly(typeof(AppDbContextFactory).Assembly);

            return new DbContextOptionsBuilder()
                .UseSqlServer(connectionString)
                .UseModel(mb.FinalizeModel())
                .Options;
        }
    }
}