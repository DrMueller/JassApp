using Microsoft.EntityFrameworkCore;

namespace JassApp.DataAccess.DbContexts.Contexts
{
    public class DbSetProxy<TEntity>(DbSet<TEntity> dbSet) : IDbSetProxy<TEntity>
        where TEntity : class
    {
        public async Task AddAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
        }

        public IQueryable<TEntity> AsNoTracking()
        {
            return dbSet.AsNoTracking();
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return dbSet;
        }

        public void Remove(TEntity entity)
        {
            dbSet.Remove(entity);
        }
    }
}