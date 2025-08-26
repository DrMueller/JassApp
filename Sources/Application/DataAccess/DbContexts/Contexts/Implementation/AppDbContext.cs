using JassApp.DataAccess.Tables.Base;
using Microsoft.EntityFrameworkCore;

namespace JassApp.DataAccess.DbContexts.Contexts.Implementation
{
    public class AppDbContext(DbContextOptions options) : DbContext(options), IAppDbContext
    {
        public IDbSetProxy<TTable> DbSet<TTable>() where TTable : TableBase
        {
            var set = Set<TTable>();

            return new DbSetProxy<TTable>(set);
        }

        public IQueryable<TTable> Query<TTable>() where TTable : TableBase
        {
            return Set<TTable>().AsQueryable();
        }

        // Only called if the models are not configured
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}