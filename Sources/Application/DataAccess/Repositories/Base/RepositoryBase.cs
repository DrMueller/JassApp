using System.Linq.Expressions;
using JassApp.DataAccess.DbContexts.Contexts;
using JassApp.DataAccess.Tables.Base;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace JassApp.DataAccess.Repositories.Base
{
    public abstract class RepositoryBase : IRepositoryBase
    {
        private IAppDbContext _dbContext = null!;

        public void Initialize(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected async Task AddAsync<T>(T entity)
            where T : TableBase
        {
            await _dbContext.DbSet<T>().AddAsync(entity);
        }

        protected async Task AddRangeAsync<T>(ICollection<T> entities)
            where T : TableBase
        {
            foreach (var entity in entities)
            {
                await _dbContext.DbSet<T>().AddAsync(entity);
            }
        }

        protected IQueryable<T> Query<T>() where T : TableBase
        {
            return _dbContext.DbSet<T>().AsQueryable();
        }

        protected async Task<T> QueryFirstAsync<T>(Expression<Func<T, bool>> predicate)
            where T : TableBase
        {
            return await Query<T>().FirstAsync(predicate);
        }

        protected async Task<T> QuerySingleAsync<T>(Expression<Func<T, bool>> predicate)
            where T : TableBase
        {
            return await Query<T>().SingleAsync(predicate);
        }

        protected async Task<T?> QuerySingleOrDefaultAsync<T>(Expression<Func<T, bool>> predicate)
            where T : TableBase
        {
            return await Query<T>().SingleOrDefaultAsync(predicate);
        }

        [UsedImplicitly]
        protected void Remove<T>(T entity)
            where T : TableBase
        {
            _dbContext.DbSet<T>().Remove(entity);
        }

        protected void RemoveRange<T>(ICollection<T> entities)
            where T : TableBase
        {
            foreach (var entity in entities)
            {
                _dbContext.DbSet<T>().Remove(entity);
            }
        }
    }
}