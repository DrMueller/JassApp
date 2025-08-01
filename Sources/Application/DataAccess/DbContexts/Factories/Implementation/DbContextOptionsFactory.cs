using System.Linq.Expressions;
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
            SetDeletedFilters(mb);

            return new DbContextOptionsBuilder()
                .UseSqlServer(connectionString)
                .UseModel(mb.FinalizeModel())
                .Options;
        }

        public static void SetDeletedFilters(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var parameter = Expression.Parameter(entityType.ClrType, "p");

                var deletedPropertyName = entityType.ClrType.Name + "_Deleted";
                var deletedProperty = entityType.ClrType.GetProperty(deletedPropertyName);
                if (deletedProperty == null)
                {
                    continue;
                }

                var deletedCheck = Expression.Lambda(Expression.Equal(Expression.Property(parameter, deletedPropertyName), Expression.Constant(null)), parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(deletedCheck);
            }
        }
    }
}